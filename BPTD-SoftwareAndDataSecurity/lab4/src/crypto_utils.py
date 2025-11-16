# -*- coding: utf-8 -*-
import os
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes
from cryptography.hazmat.primitives import padding
from cryptography.hazmat.backends import default_backend
import base64

class AESCipher:
    def __init__(self, key: bytes):
        if len(key) not in [16, 24, 32]:
            raise ValueError("Key length must be 16, 24 or 32 bytes")
        self.key = key
        self.backend = default_backend()

    def encrypt(self, plaintext: str) -> str:
        iv = os.urandom(16)
        cipher = Cipher(algorithms.AES(self.key), modes.CBC(iv), backend=self.backend)
        encryptor = cipher.encryptor()
        padder = padding.PKCS7(128).padder()
        padded_data = padder.update(plaintext.encode('utf-8')) + padder.finalize()
        ciphertext = encryptor.update(padded_data) + encryptor.finalize()
        return base64.b64encode(iv + ciphertext).decode('utf-8')

    def decrypt(self, ciphertext: str) -> str:
        try:
            data = base64.b64decode(ciphertext.encode('utf-8'))
            iv, encrypted_data = data[:16], data[16:]
            cipher = Cipher(algorithms.AES(self.key), modes.CBC(iv), backend=self.backend)
            decryptor = cipher.decryptor()
            padded_plaintext = decryptor.update(encrypted_data) + decryptor.finalize()
            unpadder = padding.PKCS7(128).unpadder()
            plaintext = unpadder.update(padded_plaintext) + unpadder.finalize()
            return plaintext.decode('utf-8')
        except Exception as e:
            raise ValueError(f"Decryption error: {e}")
