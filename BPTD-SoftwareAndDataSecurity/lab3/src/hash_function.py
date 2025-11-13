def custom_hash(data: bytes, bit_length: int = 8) -> int:
    """
    Calculate custom hash of the data with specified bit length.
    Uses aggressive mixing to ensure avalanche effect (≥30% bit changes).

    Args:
        data: Input data as bytes
        bit_length: Length of hash output (2, 4, or 8 bits)

    Returns:
        Hash value as integer

    Raises:
        ValueError: If bit_length is not 2, 4, or 8
    """
    if bit_length not in [2, 4, 8]:
        raise ValueError("bit_length must be 2, 4, or 8")

    # Initialize hash state with different values for each bit length
    if bit_length == 2:
        hash_state = 0x3  # 11 in binary
        prime1, prime2 = 3, 2
    elif bit_length == 4:
        hash_state = 0x7  # 0111 in binary
        prime1, prime2 = 11, 13
    else:  # 8 bits
        hash_state = 0x5A  # 01011010 in binary
        prime1, prime2 = 31, 37

    mask = (1 << bit_length) - 1

    # Process each byte with aggressive mixing
    for i, byte in enumerate(data):
        # Multiple rounds of mixing for each byte
        temp = byte

        # Round 1: XOR with position
        temp ^= (i * prime1) & 0xFF
        hash_state ^= temp & mask

        # Round 2: Multiply and rotate
        hash_state = (hash_state * prime1) & mask
        if bit_length == 8:
            hash_state = ((hash_state << 3) | (hash_state >> 5)) & mask
        elif bit_length == 4:
            hash_state = ((hash_state << 1) | (hash_state >> 3)) & mask
        else:  # 2 bits
            hash_state = ((hash_state << 1) | (hash_state >> 1)) & mask

        # Round 3: XOR with different parts of byte
        hash_state ^= (byte >> (i % 3)) & mask
        hash_state ^= (byte << (i % 3)) & mask

        # Round 4: Non-linear mixing
        hash_state = (hash_state + ((byte * prime2) & mask)) & mask
        hash_state ^= ((byte ^ prime2) >> (i % 4)) & mask

        # Round 5: Additional rotation
        if bit_length >= 4:
            hash_state = ((hash_state << 2) | (hash_state >> (bit_length - 2))) & mask

    # Final aggressive mixing rounds
    for _ in range(3):
        hash_state = (hash_state * prime1) & mask
        hash_state ^= (hash_state >> 1) & mask
        hash_state = ((hash_state << 1) | (hash_state >> (bit_length - 1))) & mask
        hash_state ^= len(data) & mask

    return hash_state & mask


def hash_to_binary_string(hash_value: int, bit_length: int) -> str:
    """
    Convert hash value to binary string representation.

    Args:
        hash_value: Hash value as integer
        bit_length: Length in bits

    Returns:
        Binary string representation
    """
    return format(hash_value, f'0{bit_length}b')


def calculate_file_hash(file_path: str, bit_length: int = 8) -> tuple[int, str]:
    """
    Calculate hash of a file.

    Args:
        file_path: Path to the file
        bit_length: Length of hash output (2, 4, or 8 bits)

    Returns:
        Tuple of (hash_value, binary_string)
    """
    with open(file_path, 'rb') as f:
        data = f.read()

    hash_value = custom_hash(data, bit_length)
    binary_str = hash_to_binary_string(hash_value, bit_length)

    return hash_value, binary_str


def test_avalanche_effect(test_data: bytes, bit_length: int = 8, num_tests: int = 100) -> float:
    """
    Test the avalanche effect - how many bits change when input changes.

    Args:
        test_data: Test data
        bit_length: Hash bit length
        num_tests: Number of tests to run

    Returns:
        Average percentage of bits changed
    """
    import random

    original_hash = custom_hash(test_data, bit_length)
    total_changes = 0

    for _ in range(num_tests):
        # Create modified data by changing one random byte
        modified_data = bytearray(test_data)
        if len(modified_data) > 0:
            random_pos = random.randint(0, len(modified_data) - 1)
            # Change the byte
            modified_data[random_pos] = (modified_data[random_pos] + random.randint(1, 255)) % 256

            # Calculate new hash
            modified_hash = custom_hash(bytes(modified_data), bit_length)

            # Count different bits
            xor_result = original_hash ^ modified_hash
            bits_changed = bin(xor_result).count('1')
            total_changes += bits_changed

    avg_bits_changed = total_changes / num_tests
    percentage = (avg_bits_changed / bit_length) * 100

    return percentage


if __name__ == "__main__":
    # Test the hash function
    test_string = b"Hello, World!"

    print("Testing custom hash function:")
    print(f"Input: {test_string}")
    print()

    for bits in [2, 4, 8]:
        hash_val, binary = calculate_file_hash.__wrapped__(test_string, bits)
        print(f"{bits}-bit hash: {hash_val} (binary: {binary})")

        # Test avalanche effect
        avalanche = test_avalanche_effect(test_string, bits, 100)
        print(f"  Avalanche effect: {avalanche:.2f}% of bits change on average")
        print()
