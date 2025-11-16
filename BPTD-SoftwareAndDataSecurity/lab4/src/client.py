# -*- coding: utf-8 -*-
import socket
import threading
from datetime import datetime
from typing import List

from rich.console import Console
from rich.prompt import Prompt
from rich.panel import Panel
from rich.table import Table
from rich.text import Text
from rich import box

from diffie_hellman import DiffieHellman
from crypto_utils import AESCipher
from utils import send_message, receive_message, create_message, MessageType

class ChatClient:
    def __init__(self, host: str, port: int):
        self.host, self.port = host, port
        self.socket = None
        self.username = None
        self.running = False
        self.dh = None
        self.session_key = None
        self.group_key = None
        self.group_cipher = None
        self.messages: List[dict] = []
        self.users: List[str] = []
        self.messages_lock = threading.Lock()
        self.console = Console()

    def connect(self):
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
            self.socket.connect((self.host, self.port))
            self.console.print("\n[bold cyan]Welcome to Secure Chat![/bold cyan]")
            self.username = Prompt.ask("[yellow]Enter your name[/yellow]")
            self.console.print("\n[bold]Step 1: Diffie-Hellman Key Exchange[/bold]")
            msg = receive_message(self.socket)
            if not msg or msg["type"] != MessageType.KEY_EXCHANGE_START:
                raise Exception("Failed to get DH parameters")
            prime, generator, server_public_key = msg["prime"], msg["generator"], msg["server_public_key"]
            self.console.print(f"[green]✓[/green] Received DH parameters from server")
            self.dh = DiffieHellman(prime=prime, generator=generator)
            private_key, public_key = self.dh.generate_keypair()
            self.console.print(f"[green]✓[/green] Generated keypair")
            response = create_message(MessageType.PUBLIC_KEY, public_key=public_key, username=self.username)
            send_message(self.socket, response)
            self.console.print(f"[green]✓[/green] Sent public key to server")
            session_secret = self.dh.compute_shared_secret(server_public_key)
            self.session_key = self.dh.derive_key(session_secret)
            self.console.print(f"[green]✓[/green] Computed session key")
            self.console.print("\n[bold]Step 2: Receiving Group Key[/bold]")
            msg = receive_message(self.socket)
            if not msg or msg["type"] != MessageType.GROUP_KEY:
                raise Exception("Failed to get group key")
            session_cipher = AESCipher(self.session_key)
            group_key_hex = session_cipher.decrypt(msg["encrypted_group_key"])
            self.group_key = bytes.fromhex(group_key_hex)
            self.group_cipher = AESCipher(self.group_key)
            self.console.print(f"[green]✓[/green] Received and decrypted group key")
            self.console.print("\n[bold green]✓ Secure connection established![/bold green]")
            self.console.print("[dim]Press Enter to continue...[/dim]")
            input()
            return True
        except Exception as e:
            self.console.print(f"[bold red]Connection error: {e}[/bold red]")
            return False

    def receive_messages(self):
        while self.running:
            try:
                msg = receive_message(self.socket)
                if not msg: break
                if msg["type"] == MessageType.CHAT_MESSAGE:
                    encrypted_content = msg["encrypted_content"]
                    decrypted_content = self.group_cipher.decrypt(encrypted_content)
                    username = msg["username"]
                    timestamp = msg.get("timestamp", "")
                    time_str = timestamp[11:19] if len(timestamp) >= 19 else ""

                    if username != self.username:
                        print(f"\r[{time_str}] {username}: {decrypted_content}")
                        self.display_footer()

                    with self.messages_lock:
                        self.messages.append({
                            "username": username,
                            "content": decrypted_content,
                            "timestamp": timestamp
                        })

                elif msg["type"] == MessageType.USER_JOINED:
                    timestamp = msg.get("timestamp", "")
                    time_str = timestamp[11:19] if len(timestamp) >= 19 else ""
                    print(f"\r[{time_str}] • {msg['username']} joined the chat")
                    self.display_footer()

                    with self.messages_lock:
                        self.messages.append({
                            "system": True,
                            "content": f"{msg['username']} joined the chat",
                            "timestamp": timestamp
                        })
                        if msg["username"] not in self.users:
                            self.users.append(msg["username"])

                elif msg["type"] == MessageType.USER_LEFT:
                    timestamp = msg.get("timestamp", "")
                    time_str = timestamp[11:19] if len(timestamp) >= 19 else ""
                    print(f"\r[{time_str}] • {msg['username']} left the chat")
                    self.display_footer()

                    with self.messages_lock:
                        self.messages.append({
                            "system": True,
                            "content": f"{msg['username']} left the chat",
                            "timestamp": timestamp
                        })
                        if msg["username"] in self.users:
                            self.users.remove(msg["username"])

                elif msg["type"] == MessageType.USER_LIST:
                    self.users = msg["users"]
            except Exception as e:
                if self.running:
                    pass
                break

    def send_chat_message(self, content: str):
        try:
            timestamp = datetime.now().isoformat()
            encrypted_content = self.group_cipher.encrypt(content)
            msg = create_message(MessageType.CHAT_MESSAGE, encrypted_content=encrypted_content, timestamp=timestamp)
            send_message(self.socket, msg)

            time_str = timestamp[11:19] if len(timestamp) >= 19 else ""
            self.console.print(f"[dim][{time_str}][/dim] [bold cyan]{self.username}:[/bold cyan] [cyan]{content}[/cyan]")
        except Exception as e:
            self.console.print(f"[red]Send error: {e}[/red]")

    def display_header(self):
        header = Table.grid(padding=1)
        header.add_column(style="cyan", justify="left")
        header.add_column(style="yellow", justify="right")
        header.add_row(
            f"[bold]💬 Secure Chat - {self.username}[/bold]",
            f"👥 Users: {len(self.users)} | 🔐 AES-256"
        )
        self.console.print(Panel(header, border_style="blue", box=box.DOUBLE))

    def display_users(self):
        users_text = Text()
        for i, user in enumerate(sorted(self.users)):
            if user == self.username:
                users_text.append(f"👤 {user} (you)", style="bold cyan")
            else:
                users_text.append(f"👤 {user}", style="green")
            if i < len(self.users) - 1:
                users_text.append(" | ")
        self.console.print(Panel(users_text, title="[bold]Active Users[/bold]", border_style="green", box=box.ROUNDED))

    def display_footer(self):
        self.console.print(f"\n[cyan]>[/cyan] ", end="")

    def run(self):
        if not self.connect(): 
            return
        
        self.running = True
        threading.Thread(target=self.receive_messages, daemon=True).start()
        
        # Clear and show initial interface
        self.console.clear()
        self.display_header()
        self.console.print()
        self.display_users()
        self.console.print()
        self.console.print("[dim]Type your messages below. Commands: /quit (exit), /users (show users), /clear (clear screen)[/dim]")
        self.console.print("[dim]" + "─" * 80 + "[/dim]\n")
        
        try:
            while self.running:
                # Get input
                try:
                    self.display_footer()
                    message = input()
                    
                    if message == "/quit":
                        break
                    elif message == "/users":
                        self.console.print()
                        self.display_users()
                        self.console.print()
                    elif message == "/clear":
                        self.console.clear()
                        self.display_header()
                        self.console.print()
                        self.display_users()
                        self.console.print()
                        # Show last 10 messages
                        with self.messages_lock:
                            for msg in self.messages[-10:]:
                                timestamp = msg.get("timestamp", "")[:19].replace("T", " ")
                                time_str = timestamp[11:19] if len(timestamp) >= 19 else ""
                                if msg.get("system"):
                                    self.console.print(f"[dim][{time_str}][/dim] [italic yellow]• {msg['content']}[/italic yellow]")
                                else:
                                    username = msg["username"]
                                    content = msg["content"]
                                    if username == self.username:
                                        self.console.print(f"[dim][{time_str}][/dim] [bold cyan]{username}:[/bold cyan] [cyan]{content}[/cyan]")
                                    else:
                                        self.console.print(f"[dim][{time_str}][/dim] [bold green]{username}:[/bold green] {content}")
                        self.console.print()
                    elif message.strip():
                        self.send_chat_message(message)
                        
                except (EOFError, KeyboardInterrupt):
                    break
        finally:
            self.running = False
            if self.socket: 
                self.socket.close()
            self.console.print("\n[yellow]Disconnected. Goodbye![/yellow]")

def main():
    console = Console()
    console.print("[bold cyan]" + "="*60 + "[/bold cyan]")
    console.print("[bold cyan]  SECURE CHAT CLIENT - Lab 4[/bold cyan]")
    console.print("[bold cyan]" + "="*60 + "[/bold cyan]\n")
    host = Prompt.ask("[yellow]Server address[/yellow]", default="127.0.0.1")
    port = Prompt.ask("[yellow]Server port[/yellow]", default="5555")
    try: 
        port = int(port)
    except ValueError:
        console.print("[red]Invalid port![/red]")
        return
    client = ChatClient(host, port)
    client.run()

if __name__ == "__main__":
    main()
