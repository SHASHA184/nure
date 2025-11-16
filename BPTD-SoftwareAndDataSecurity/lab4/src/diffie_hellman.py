# -*- coding: utf-8 -*-
import secrets
import hashlib
from typing import Tuple

def is_prime(n: int, k: int = 10) -> bool:
    if n < 2: return False
    if n == 2 or n == 3: return True
    if n % 2 == 0: return False
    r, d = 0, n - 1
    while d % 2 == 0:
        r += 1
        d //= 2
    for _ in range(k):
        a = secrets.randbelow(n - 3) + 2
        x = pow(a, d, n)
        if x == 1 or x == n - 1: continue
        for _ in range(r - 1):
            x = pow(x, 2, n)
            if x == n - 1: break
        else: return False
    return True

def generate_prime(bits: int = 512) -> int:
    while True:
        candidate = secrets.randbits(bits)
        candidate |= (1 << bits - 1) | 1
        if is_prime(candidate):
            return candidate

def find_primitive_root(p: int) -> int:
    if p == 2:
        return 1
    phi = p - 1
    prime_factors = []
    n = phi
    i = 2
    while i * i <= n:
        if n % i == 0:
            prime_factors.append(i)
            while n % i == 0:
                n //= i
        i += 1
    if n > 1:
        prime_factors.append(n)
    for g in range(2, p):
        is_prim = True
        for factor in prime_factors:
            if pow(g, phi // factor, p) == 1:
                is_prim = False
                break
        if is_prim:
            return g
    return 2

class DiffieHellman:
    def __init__(self, prime: int = None, generator: int = None, bits: int = 512):
        self.prime = generate_prime(bits) if prime is None else prime
        self.generator = find_primitive_root(self.prime) if generator is None else generator
        self.private_key = None
        self.public_key = None

    def generate_keypair(self) -> Tuple[int, int]:
        self.private_key = secrets.randbelow(self.prime - 3) + 2
        self.public_key = pow(self.generator, self.private_key, self.prime)
        return self.private_key, self.public_key

    def compute_shared_secret(self, other_public_key: int) -> int:
        if self.private_key is None:
            raise ValueError("Generate keypair first")
        return pow(other_public_key, self.private_key, self.prime)

    def derive_key(self, shared_secret: int, key_length: int = 32) -> bytes:
        secret_bytes = shared_secret.to_bytes((shared_secret.bit_length() + 7) // 8, byteorder='big')
        derived_key = hashlib.sha256(secret_bytes).digest()
        if key_length > 32:
            derived_key += hashlib.sha256(derived_key).digest()
        return derived_key[:key_length]

    def get_public_parameters(self) -> Tuple[int, int]:
        return self.prime, self.generator
