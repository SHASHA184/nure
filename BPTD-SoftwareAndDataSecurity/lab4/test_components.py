#!/usr/bin/env python3
"""Тест основних компонентів системи"""

import sys
import os
sys.path.insert(0, os.path.join(os.path.dirname(__file__), 'src'))

print("="*60)
print("  ТЕСТ КОМПОНЕНТІВ БЕЗПЕЧНОГО ЧАТУ")
print("="*60)

# Тест 1: Імпорти
print("\n[1] Перевірка імпортів...")
try:
    from diffie_hellman import DiffieHellman
    from crypto_utils import AESCipher
    from utils import MessageType, create_message, send_message, receive_message
    print("    ✓ Всі модулі успішно імпортовано")
except Exception as e:
    print(f"    ✗ Помилка імпорту: {e}")
    sys.exit(1)

# Тест 2: AES шифрування
print("\n[2] Перевірка AES шифрування...")
try:
    key = b"0" * 32  # 256-bit ключ
    cipher = AESCipher(key)
    plaintext = "Привіт, світ! 🔐"
    encrypted = cipher.encrypt(plaintext)
    decrypted = cipher.decrypt(encrypted)
    assert plaintext == decrypted, "Розшифроване не співпадає з оригіналом"
    print(f"    ✓ Оригінал: {plaintext}")
    print(f"    ✓ Зашифровано: {encrypted[:50]}...")
    print(f"    ✓ Розшифровано: {decrypted}")
except Exception as e:
    print(f"    ✗ Помилка: {e}")
    sys.exit(1)

# Тест 3: Diffie-Hellman (мала розрядність для швидкості)
print("\n[3] Перевірка Diffie-Hellman (128 біт для швидкості)...")
try:
    # Створюємо двох учасників
    print("    Генерація параметрів DH для Alice...")
    alice_dh = DiffieHellman(bits=128)
    alice_private, alice_public = alice_dh.generate_keypair()
    prime, generator = alice_dh.get_public_parameters()
    print(f"    ✓ Alice: приватний={alice_private}, публічний={alice_public}")

    print("    Генерація ключів для Bob...")
    bob_dh = DiffieHellman(prime=prime, generator=generator)
    bob_private, bob_public = bob_dh.generate_keypair()
    print(f"    ✓ Bob: приватний={bob_private}, публічний={bob_public}")

    # Обмін ключами
    alice_secret = alice_dh.compute_shared_secret(bob_public)
    bob_secret = bob_dh.compute_shared_secret(alice_public)

    alice_key = alice_dh.derive_key(alice_secret)
    bob_key = bob_dh.derive_key(bob_secret)

    assert alice_key == bob_key, "Ключі не співпадають!"
    print(f"    ✓ Спільний ключ: {alice_key.hex()[:32]}...")
    print(f"    ✓ Довжина: {len(alice_key)} байт")
except Exception as e:
    print(f"    ✗ Помилка: {e}")
    import traceback
    traceback.print_exc()
    sys.exit(1)

# Тест 4: Повний цикл (DH + AES)
print("\n[4] Перевірка повного циклу (DH + AES)...")
try:
    # Використовуємо спільний ключ з попереднього тесту
    alice_cipher = AESCipher(alice_key)
    bob_cipher = AESCipher(bob_key)

    message = "Секретне повідомлення від Alice до Bob! 🔒"
    encrypted_msg = alice_cipher.encrypt(message)
    decrypted_msg = bob_cipher.decrypt(encrypted_msg)

    assert message == decrypted_msg, "Повідомлення не співпадають"
    print(f"    ✓ Alice шифрує: {message}")
    print(f"    ✓ Зашифровано: {encrypted_msg[:50]}...")
    print(f"    ✓ Bob дешифрує: {decrypted_msg}")
except Exception as e:
    print(f"    ✗ Помилка: {e}")
    sys.exit(1)

# Тест 5: Протокол повідомлень
print("\n[5] Перевірка протоколу повідомлень...")
try:
    msg1 = create_message(MessageType.CHAT_MESSAGE, content="Hello", username="Alice")
    assert msg1["type"] == MessageType.CHAT_MESSAGE
    assert msg1["username"] == "Alice"
    print(f"    ✓ Повідомлення чату: {msg1}")

    msg2 = create_message(MessageType.KEY_EXCHANGE_START, prime=12345, generator=2)
    assert msg2["type"] == MessageType.KEY_EXCHANGE_START
    print(f"    ✓ Обмін ключами: {msg2}")
except Exception as e:
    print(f"    ✗ Помилка: {e}")
    sys.exit(1)

print("\n" + "="*60)
print("  ✅ ВСІ ТЕСТИ ПРОЙДЕНО УСПІШНО!")
print("="*60)
print("\n📋 Висновок:")
print("  • Diffie-Hellman реалізовано коректно")
print("  • AES-256 шифрування працює правильно")
print("  • Протокол обміну повідомленнями готовий")
print("  • Система готова до тестування з реальними клієнтами")
print()
