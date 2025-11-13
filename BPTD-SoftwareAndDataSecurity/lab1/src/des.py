import math
from typing import List
from des_tables import *


class DES:
    def __init__(self, key: bytes):
        if len(key) != 8:
            raise ValueError("Ключ повинен бути 8 байт")

        self.key = key
        self.round_keys = []
        self.round_entropies = []

        if self.is_weak_key(key):
            raise ValueError("Слабкий ключ! Виберіть інший.")

        self.round_keys = self._generate_round_keys()

    @staticmethod
    def is_weak_key(key: bytes) -> bool:
        """Перевірка на слабкі ключі"""
        key_int = int.from_bytes(key, byteorder='big')

        if key_int in WEAK_KEYS:
            return True

        for pair in SEMI_WEAK_KEYS:
            if key_int == pair[0] or key_int == pair[1]:
                return True

        return False

    @staticmethod
    def _bytes_to_bits(data: bytes) -> List[int]:
        """Конвертація байтів у біти"""
        bits = []
        for byte in data:
            for i in range(7, -1, -1):
                bits.append((byte >> i) & 1)
        return bits

    @staticmethod
    def _bits_to_bytes(bits: List[int]) -> bytes:
        """Конвертація бітів у байти"""
        byte_array = []
        for i in range(0, len(bits), 8):
            byte = 0
            for j in range(8):
                if i + j < len(bits):
                    byte = (byte << 1) | bits[i + j]
            byte_array.append(byte)
        return bytes(byte_array)

    @staticmethod
    def _permute(bits: List[int], table: List[int]) -> List[int]:
        """Перестановка бітів згідно таблиці"""
        return [bits[i - 1] for i in table]

    @staticmethod
    def _left_shift(bits: List[int], n: int) -> List[int]:
        """Циклічний зсув вліво"""
        return bits[n:] + bits[:n]

    @staticmethod
    def _xor(bits1: List[int], bits2: List[int]) -> List[int]:
        """XOR двох списків бітів"""
        return [b1 ^ b2 for b1, b2 in zip(bits1, bits2)]

    @staticmethod
    def _calculate_entropy(bits: List[int]) -> float:
        """Обчислення ентропії"""
        if not bits:
            return 0.0

        ones = sum(bits)
        zeros = len(bits) - ones

        p1 = ones / len(bits) if len(bits) > 0 else 0
        p0 = zeros / len(bits) if len(bits) > 0 else 0

        entropy = 0.0
        if p0 > 0:
            entropy -= p0 * math.log2(p0)
        if p1 > 0:
            entropy -= p1 * math.log2(p1)

        return entropy

    def _generate_round_keys(self) -> List[List[int]]:
        """Генерація 16 раундових ключів"""
        key_bits = self._bytes_to_bits(self.key)

        # PC1
        key_56 = self._permute(key_bits, PC1)

        # Розділення на C і D
        C = key_56[:28]
        D = key_56[28:]

        round_keys = []

        for i in range(16):
            # Зсув
            C = self._left_shift(C, SHIFTS[i])
            D = self._left_shift(D, SHIFTS[i])

            # PC2
            CD = C + D
            round_key = self._permute(CD, PC2)
            round_keys.append(round_key)

        return round_keys

    def _s_box_substitution(self, bits: List[int]) -> List[int]:
        """Підстановка через S-boxes"""
        result = []

        for i in range(8):
            block = bits[i * 6:(i + 1) * 6]

            row = (block[0] << 1) | block[5]
            col = (block[1] << 3) | (block[2] << 2) | (block[3] << 1) | block[4]

            val = S_BOXES[i][row][col]

            for j in range(3, -1, -1):
                result.append((val >> j) & 1)

        return result

    def _f_function(self, right: List[int], round_key: List[int]) -> List[int]:
        """Функція f мережі Фейстеля"""
        # Розширення E
        expanded = self._permute(right, E)

        # XOR з ключем
        xored = self._xor(expanded, round_key)

        # S-boxes
        substituted = self._s_box_substitution(xored)

        # Перестановка P
        result = self._permute(substituted, P)

        return result

    def _des_block(self, block: bytes, decrypt: bool = False) -> bytes:
        """Шифрування/розшифрування одного блоку"""
        bits = self._bytes_to_bits(block)

        # Початкова перестановка
        bits = self._permute(bits, IP)

        left = bits[:32]
        right = bits[32:]

        self.round_entropies = []

        keys = self.round_keys[::-1] if decrypt else self.round_keys

        # 16 раундів
        for i in range(16):
            old_right = right[:]

            f_result = self._f_function(right, keys[i])
            right = self._xor(left, f_result)
            left = old_right

            # Розрахунок ентропії
            entropy = self._calculate_entropy(right)
            self.round_entropies.append({
                'round': i + 1,
                'entropy': entropy,
                'ones': sum(right),
                'zeros': len(right) - sum(right)
            })

        # Кінцева перестановка
        combined = right + left
        final_bits = self._permute(combined, IP_INV)

        return self._bits_to_bytes(final_bits)

    def encrypt(self, plaintext: bytes) -> bytes:
        """Шифрування"""
        # Padding
        padding_length = 8 - (len(plaintext) % 8)
        plaintext = plaintext + bytes([padding_length] * padding_length)

        ciphertext = b''
        for i in range(0, len(plaintext), 8):
            block = plaintext[i:i + 8]
            ciphertext += self._des_block(block, decrypt=False)

        return ciphertext

    def decrypt(self, ciphertext: bytes) -> bytes:
        """Розшифрування"""
        if len(ciphertext) % 8 != 0:
            raise ValueError("Неправильна довжина")

        plaintext = b''
        for i in range(0, len(ciphertext), 8):
            block = ciphertext[i:i + 8]
            plaintext += self._des_block(block, decrypt=True)

        # Видалення padding
        padding_length = plaintext[-1]
        plaintext = plaintext[:-padding_length]

        return plaintext

    def get_last_round_entropies(self) -> List[dict]:
        """Отримання ентропій"""
        return self.round_entropies


def main():
    """Тест"""
    key = b'DESCRYPT'
    plaintext = b'Hello, DES!'

    print("=" * 60)
    print("Тест DES")
    print("=" * 60)
    print(f"Ключ: {key}")
    print(f"Текст: {plaintext}")
    print()

    try:
        des = DES(key)

        ciphertext = des.encrypt(plaintext)
        print(f"Шифротекст (hex): {ciphertext.hex()}")
        print()

        print("Ентропія по раундах:")
        print("-" * 60)
        for info in des.get_last_round_entropies():
            print(f"Раунд {info['round']:2d}: "
                  f"Ентропія = {info['entropy']:.6f}, "
                  f"1 = {info['ones']:2d}, "
                  f"0 = {info['zeros']:2d}")
        print()

        decrypted = des.decrypt(ciphertext)
        print(f"Розшифроване: {decrypted}")
        print()

        if plaintext == decrypted:
            print("Тест пройдено!")
        else:
            print("Помилка!")

    except ValueError as e:
        print(f"Помилка: {e}")


if __name__ == "__main__":
    main()
