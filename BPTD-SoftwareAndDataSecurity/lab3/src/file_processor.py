import os
from pathlib import Path
from typing import Optional
from hash_function import custom_hash, hash_to_binary_string


class FileProcessor:
    """Process files and calculate their hashes"""

    SUPPORTED_EXTENSIONS = {
        'document': ['.docx', '.doc'],
        'code': ['.py', '.js', '.java', '.cpp', '.c', '.go', '.cs', '.txt'],
        'image': ['.png', '.jpg', '.jpeg', '.gif', '.bmp']
    }

    @staticmethod
    def get_file_type(file_path: str) -> Optional[str]:
        """
        Determine file type based on extension.

        Args:
            file_path: Path to the file

        Returns:
            File type ('document', 'code', 'image') or None if unsupported
        """
        ext = Path(file_path).suffix.lower()

        for file_type, extensions in FileProcessor.SUPPORTED_EXTENSIONS.items():
            if ext in extensions:
                return file_type

        return None

    @staticmethod
    def calculate_hash(file_path: str, bit_length: int = 8) -> dict:
        """
        Calculate hash of a file.

        Args:
            file_path: Path to the file
            bit_length: Hash bit length (2, 4, or 8)

        Returns:
            Dictionary with hash information
        """
        if not os.path.exists(file_path):
            raise FileNotFoundError(f"File not found: {file_path}")

        # Read file content
        with open(file_path, 'rb') as f:
            data = f.read()

        # Calculate hash
        hash_value = custom_hash(data, bit_length)
        binary_str = hash_to_binary_string(hash_value, bit_length)

        # Get file info
        file_type = FileProcessor.get_file_type(file_path)
        file_size = os.path.getsize(file_path)

        return {
            'file_path': file_path,
            'file_name': os.path.basename(file_path),
            'file_type': file_type,
            'file_size': file_size,
            'bit_length': bit_length,
            'hash_value': hash_value,
            'hash_binary': binary_str,
            'data': data  # Keep data for comparison
        }

    @staticmethod
    def compare_hashes(file1: str, file2: str, bit_length: int = 8) -> dict:
        """
        Compare hashes of two files.

        Args:
            file1: Path to first file
            file2: Path to second file
            bit_length: Hash bit length

        Returns:
            Comparison results
        """
        hash1 = FileProcessor.calculate_hash(file1, bit_length)
        hash2 = FileProcessor.calculate_hash(file2, bit_length)

        # Check if files are identical
        files_identical = hash1['data'] == hash2['data']

        # Check if hashes match
        hashes_match = hash1['hash_value'] == hash2['hash_value']

        # Calculate difference in bits if hashes don't match
        bits_different = 0
        if not hashes_match:
            xor_result = hash1['hash_value'] ^ hash2['hash_value']
            bits_different = bin(xor_result).count('1')

        return {
            'file1': hash1,
            'file2': hash2,
            'files_identical': files_identical,
            'hashes_match': hashes_match,
            'collision': hashes_match and not files_identical,
            'bits_different': bits_different,
            'bit_length': bit_length
        }

    @staticmethod
    def format_hash_info(hash_info: dict, show_data: bool = False) -> str:
        """
        Format hash information as a readable string.

        Args:
            hash_info: Hash information dictionary
            show_data: Whether to show file data (for small files)

        Returns:
            Formatted string
        """
        lines = [
            f"File: {hash_info['file_name']}",
            f"Path: {hash_info['file_path']}",
            f"Type: {hash_info['file_type']}",
            f"Size: {hash_info['file_size']} bytes",
            f"Hash ({hash_info['bit_length']}-bit): {hash_info['hash_value']}",
            f"Binary: {hash_info['hash_binary']}"
        ]

        if show_data and hash_info['file_size'] < 200:
            try:
                data_str = hash_info['data'].decode('utf-8', errors='ignore')
                lines.append(f"Content preview: {data_str[:100]}...")
            except:
                pass

        return "\n".join(lines)


def process_directory(directory: str, bit_length: int = 8) -> list:
    """
    Process all supported files in a directory.

    Args:
        directory: Directory path
        bit_length: Hash bit length

    Returns:
        List of hash information dictionaries
    """
    results = []

    if not os.path.isdir(directory):
        raise NotADirectoryError(f"Not a directory: {directory}")

    for root, dirs, files in os.walk(directory):
        for file in files:
            file_path = os.path.join(root, file)
            file_type = FileProcessor.get_file_type(file_path)

            if file_type:
                try:
                    hash_info = FileProcessor.calculate_hash(file_path, bit_length)
                    results.append(hash_info)
                except Exception as e:
                    print(f"Error processing {file_path}: {e}")

    return results


if __name__ == "__main__":
    # Test the file processor
    print("Testing File Processor\n")

    test_files = [
        "test_files/document.docx",
        "test_files/source_code.py",
        "test_files/image.png"
    ]

    for bit_length in [2, 4, 8]:
        print(f"\n{'='*60}")
        print(f"Testing with {bit_length}-bit hash")
        print(f"{'='*60}")

        for file_path in test_files:
            if os.path.exists(file_path):
                try:
                    hash_info = FileProcessor.calculate_hash(file_path, bit_length)
                    print(f"\n{FileProcessor.format_hash_info(hash_info)}")
                except Exception as e:
                    print(f"Error: {e}")
