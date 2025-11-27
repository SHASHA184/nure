# -*- coding: utf-8 -*-
import socket
import threading
import os
from datetime import datetime
from typing import Dict
from diffie_hellman import DiffieHellman
from crypto_utils import AESCipher
from utils import send_message, receive_message, create_message, MessageType

class ChatClient:
    def __init__(self, socket: socket.socket, address: tuple, username: str):
        self.socket = socket
        self.address = address
        self.username = username
        self.public_key = None
        self.session_key = None

class ChatServer:
    def __init__(self, host: str = '0.0.0.0', port: int = 5555, dh_group: str = "modp_2048"):
        self.host, self.port = host, port
        self.server_socket = None
        print(f"[*] Loading DH parameters (RFC 3526 {dh_group})...")
        self.dh = DiffieHellman.from_rfc3526(dh_group)
        self.server_private_key, self.server_public_key = self.dh.generate_keypair()
        self.prime, self.generator = self.dh.get_public_parameters()
        print(f"[+] DH parameters loaded")
        print(f"    Q (prime) = {str(self.prime)[:50]}...")
        print(f"    A (generator) = {self.generator}")
        print(f"    Server public key = {str(self.server_public_key)[:50]}...")
        self.clients: Dict[str, ChatClient] = {}
        self.clients_lock = threading.Lock()
        self.group_key = os.urandom(32)
        self.group_cipher = AESCipher(self.group_key)
        print(f"[+] Group key generated: {self.group_key.hex()[:32]}...")

    def start(self):
        self.server_socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.server_socket.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
        self.server_socket.bind((self.host, self.port))
        self.server_socket.listen(5)
        print(f"\n[+] Server started on {self.host}:{self.port}\n")
        try:
            while True:
                client_socket, client_address = self.server_socket.accept()
                print(f"[+] New connection from {client_address}")
                threading.Thread(target=self.handle_client, args=(client_socket, client_address), daemon=True).start()
        except KeyboardInterrupt:
            print("\n[!] Stopping server...")
            self.stop()

    def stop(self):
        with self.clients_lock:
            for client in self.clients.values():
                try: client.socket.close()
                except: pass
            self.clients.clear()
        if self.server_socket: self.server_socket.close()
        print("[+] Server stopped")

    def handle_client(self, client_socket: socket.socket, client_address: tuple):
        client = None
        try:
            msg = create_message(MessageType.KEY_EXCHANGE_START, prime=self.prime, generator=self.generator, server_public_key=self.server_public_key)
            send_message(client_socket, msg)
            response = receive_message(client_socket)
            if not response or response["type"] != MessageType.PUBLIC_KEY: raise ValueError("Expected public key")
            client_public_key, username = response["public_key"], response["username"]
            with self.clients_lock:
                if username in self.clients:
                    send_message(client_socket, create_message(MessageType.ERROR, message="Username taken"))
                    client_socket.close()
                    return
            session_secret = self.dh.compute_shared_secret(client_public_key)
            session_key = self.dh.derive_key(session_secret)
            client = ChatClient(client_socket, client_address, username)
            client.public_key, client.session_key = client_public_key, session_key
            session_cipher = AESCipher(session_key)
            encrypted_group_key = session_cipher.encrypt(self.group_key.hex())
            send_message(client_socket, create_message(MessageType.GROUP_KEY, encrypted_group_key=encrypted_group_key))
            with self.clients_lock:
                self.clients[username] = client
            self.broadcast_user_joined(username)
            self.send_user_list_to_client(client)
            print(f"[+] {username} joined\n")
            while True:
                message = receive_message(client_socket)
                if not message: break
                if message["type"] == MessageType.CHAT_MESSAGE:
                    self.handle_chat_message(client, message)
        except Exception as e:
            print(f"[!] Error: {e}")
        finally:
            if client and client.username:
                with self.clients_lock:
                    if client.username in self.clients:
                        del self.clients[client.username]
                        print(f"[-] {client.username} disconnected")
                        self.broadcast_user_left(client.username)
            try: client_socket.close()
            except: pass

    def handle_chat_message(self, sender: ChatClient, message: dict):
        encrypted_content = message.get("encrypted_content")
        timestamp = message.get("timestamp", datetime.now().isoformat())
        broadcast_msg = create_message(MessageType.CHAT_MESSAGE, username=sender.username, encrypted_content=encrypted_content, timestamp=timestamp)
        try:
            decrypted = self.group_cipher.decrypt(encrypted_content)
            print(f"[{timestamp[:19]}] {sender.username}: {decrypted}")
        except:
            print(f"[{timestamp[:19]}] {sender.username}: [decrypt failed]")
        self.broadcast_message(broadcast_msg)

    def broadcast_message(self, message: dict, exclude_username: str = None):
        with self.clients_lock:
            for username, client in list(self.clients.items()):
                if username == exclude_username: continue
                try: send_message(client.socket, message)
                except Exception as e: print(f"[!] Send error to {username}: {e}")

    def broadcast_user_joined(self, username: str):
        self.broadcast_message(create_message(MessageType.USER_JOINED, username=username, timestamp=datetime.now().isoformat()), exclude_username=username)

    def broadcast_user_left(self, username: str):
        self.broadcast_message(create_message(MessageType.USER_LEFT, username=username, timestamp=datetime.now().isoformat()))

    def send_user_list_to_client(self, client: ChatClient):
        with self.clients_lock:
            usernames = list(self.clients.keys())
        try: send_message(client.socket, create_message(MessageType.USER_LIST, users=usernames))
        except Exception as e: print(f"[!] Error sending user list: {e}")

def main():
    import sys
    from dh_params import RFC3526_GROUPS
    print("="*60)
    print("  SECURE CHAT SERVER - Lab 4")
    print("="*60)
    dh_group = "modp_2048"  # Default (recommended)
    if len(sys.argv) > 1:
        group = sys.argv[1]
        if group in RFC3526_GROUPS:
            dh_group = group
            print(f"[*] Using DH group: {dh_group}")
        else:
            print(f"[!] Unknown group '{group}', available: {list(RFC3526_GROUPS.keys())}")
            print(f"[*] Using default: {dh_group}")
    server = ChatServer('0.0.0.0', 5555, dh_group)
    server.start()

if __name__ == "__main__":
    main()
