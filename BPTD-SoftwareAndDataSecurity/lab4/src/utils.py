# -*- coding: utf-8 -*-
import json
import socket
from typing import Dict, Any

class MessageType:
    KEY_EXCHANGE_START = "key_exchange_start"
    PUBLIC_KEY = "public_key"
    ALL_PUBLIC_KEYS = "all_public_keys"
    GROUP_KEY = "group_key"
    CHAT_MESSAGE = "chat_message"
    USER_JOINED = "user_joined"
    USER_LEFT = "user_left"
    USER_LIST = "user_list"
    ERROR = "error"
    ACK = "ack"

def send_message(sock: socket.socket, message: Dict[str, Any]) -> None:
    try:
        json_data = json.dumps(message)
        message_bytes = json_data.encode('utf-8')
        length = len(message_bytes)
        sock.sendall(length.to_bytes(4, byteorder='big'))
        sock.sendall(message_bytes)
    except Exception as e:
        raise ConnectionError(f"Send error: {e}")

def receive_message(sock: socket.socket) -> Dict[str, Any]:
    try:
        length_bytes = receive_exact(sock, 4)
        if not length_bytes: return None
        length = int.from_bytes(length_bytes, byteorder='big')
        message_bytes = receive_exact(sock, length)
        if not message_bytes: return None
        return json.loads(message_bytes.decode('utf-8'))
    except Exception as e:
        raise ConnectionError(f"Receive error: {e}")

def receive_exact(sock: socket.socket, n: int) -> bytes:
    data = bytearray()
    while len(data) < n:
        packet = sock.recv(n - len(data))
        if not packet: return None
        data.extend(packet)
    return bytes(data)

def create_message(msg_type: str, **kwargs) -> Dict[str, Any]:
    return {"type": msg_type, **kwargs}
