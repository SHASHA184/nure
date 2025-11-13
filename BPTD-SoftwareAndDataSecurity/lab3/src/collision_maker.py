import os
import shutil
from pathlib import Path
from typing import Optional, Tuple
from docx import Document
from PIL import Image
from PIL.PngImagePlugin import PngInfo
from hash_function import custom_hash


class CollisionMaker:
    """Generate hash collisions by modifying file metadata"""

    @staticmethod
    def create_collision(original_file: str, bit_length: int = 2,
                        max_attempts: int = 10000) -> Optional[str]:
        """
        Create a collision for the given file.

        Args:
            original_file: Path to original file
            bit_length: Hash bit length (smaller = easier to find collisions)
            max_attempts: Maximum number of attempts

        Returns:
            Path to collision file or None if failed
        """
        if not os.path.exists(original_file):
            raise FileNotFoundError(f"File not found: {original_file}")

        # Get file extension
        ext = Path(original_file).suffix.lower()

        # Calculate target hash
        with open(original_file, 'rb') as f:
            original_data = f.read()
        target_hash = custom_hash(original_data, bit_length)

        print(f"Target hash: {target_hash} ({format(target_hash, f'0{bit_length}b')})")
        print(f"Attempting to create collision (max {max_attempts} attempts)...")

        # Choose strategy based on file type
        if ext in ['.docx']:
            collision_file = CollisionMaker._create_docx_collision(
                original_file, target_hash, bit_length, max_attempts
            )
        elif ext in ['.png', '.jpg', '.jpeg']:
            collision_file = CollisionMaker._create_image_collision(
                original_file, target_hash, bit_length, max_attempts
            )
        elif ext in ['.py', '.txt', '.js', '.java', '.cpp', '.c', '.go', '.cs']:
            collision_file = CollisionMaker._create_code_collision(
                original_file, target_hash, bit_length, max_attempts
            )
        else:
            print(f"Unsupported file type: {ext}")
            return None

        return collision_file

    @staticmethod
    def _create_docx_collision(original_file: str, target_hash: int,
                               bit_length: int, max_attempts: int) -> Optional[str]:
        """Create collision for DOCX file by modifying core properties"""
        base_name = Path(original_file).stem
        collision_file = f"test_files/{base_name}_collision.docx"

        # Copy original file
        shutil.copy2(original_file, collision_file)

        # Try to find collision by modifying properties
        for attempt in range(max_attempts):
            try:
                doc = Document(collision_file)

                # Modify core properties (invisible to casual viewing)
                # Add spaces to comments
                core_props = doc.core_properties
                core_props.comments = ' ' * attempt

                # You can also modify other properties
                if attempt % 3 == 0:
                    core_props.subject = ' ' * (attempt // 3)
                if attempt % 5 == 0:
                    core_props.keywords = ' ' * (attempt // 5)

                # Save
                doc.save(collision_file)

                # Check hash
                with open(collision_file, 'rb') as f:
                    data = f.read()
                current_hash = custom_hash(data, bit_length)

                if attempt % 100 == 0:
                    print(f"  Attempt {attempt}: hash = {current_hash}")

                if current_hash == target_hash:
                    print(f"✓ Collision found after {attempt + 1} attempts!")
                    return collision_file

            except Exception as e:
                print(f"Error on attempt {attempt}: {e}")
                continue

        print(f"✗ Failed to find collision after {max_attempts} attempts")
        os.remove(collision_file)
        return None

    @staticmethod
    def _create_image_collision(original_file: str, target_hash: int,
                               bit_length: int, max_attempts: int) -> Optional[str]:
        """Create collision for image by modifying metadata"""
        base_name = Path(original_file).stem
        ext = Path(original_file).suffix
        collision_file = f"test_files/{base_name}_collision{ext}"

        # Load original image
        img = Image.open(original_file)

        for attempt in range(max_attempts):
            try:
                # Create metadata
                metadata = PngInfo()

                # Add invisible metadata
                metadata.add_text("Comment", " " * attempt)
                metadata.add_text("Software", " " * (attempt // 2))
                metadata.add_text("Author", " " * (attempt // 3))

                # Save with metadata
                if ext.lower() == '.png':
                    img.save(collision_file, pnginfo=metadata)
                else:
                    # For JPEG, use exif
                    img.save(collision_file, quality=95)

                # Check hash
                with open(collision_file, 'rb') as f:
                    data = f.read()
                current_hash = custom_hash(data, bit_length)

                if attempt % 100 == 0:
                    print(f"  Attempt {attempt}: hash = {current_hash}")

                if current_hash == target_hash:
                    print(f"✓ Collision found after {attempt + 1} attempts!")
                    return collision_file

            except Exception as e:
                if attempt == 0:
                    print(f"Error on attempt {attempt}: {e}")
                continue

        print(f"✗ Failed to find collision after {max_attempts} attempts")
        if os.path.exists(collision_file):
            os.remove(collision_file)
        return None

    @staticmethod
    def _create_code_collision(original_file: str, target_hash: int,
                              bit_length: int, max_attempts: int) -> Optional[str]:
        """Create collision for code file by adding comments"""
        base_name = Path(original_file).stem
        ext = Path(original_file).suffix
        collision_file = f"test_files/{base_name}_collision{ext}"

        # Read original content
        with open(original_file, 'r', encoding='utf-8') as f:
            original_content = f.read()

        # Determine comment style
        if ext in ['.py']:
            comment_prefix = '#'
        elif ext in ['.js', '.java', '.cpp', '.c', '.go', '.cs']:
            comment_prefix = '//'
        else:
            comment_prefix = '#'

        for attempt in range(max_attempts):
            try:
                # Add invisible comment with spaces
                modified_content = original_content + f"\n{comment_prefix} " + " " * attempt + "\n"

                # Write to file
                with open(collision_file, 'w', encoding='utf-8') as f:
                    f.write(modified_content)

                # Check hash
                with open(collision_file, 'rb') as f:
                    data = f.read()
                current_hash = custom_hash(data, bit_length)

                if attempt % 100 == 0:
                    print(f"  Attempt {attempt}: hash = {current_hash}")

                if current_hash == target_hash:
                    print(f"✓ Collision found after {attempt + 1} attempts!")
                    return collision_file

            except Exception as e:
                print(f"Error on attempt {attempt}: {e}")
                continue

        print(f"✗ Failed to find collision after {max_attempts} attempts")
        if os.path.exists(collision_file):
            os.remove(collision_file)
        return None

    @staticmethod
    def verify_collision(original_file: str, collision_file: str,
                        bit_length: int) -> dict:
        """
        Verify that a collision exists between two files.

        Args:
            original_file: Path to original file
            collision_file: Path to collision file
            bit_length: Hash bit length

        Returns:
            Dictionary with verification results
        """
        # Read both files
        with open(original_file, 'rb') as f:
            original_data = f.read()
        with open(collision_file, 'rb') as f:
            collision_data = f.read()

        # Calculate hashes
        original_hash = custom_hash(original_data, bit_length)
        collision_hash = custom_hash(collision_data, bit_length)

        # Check if files are identical
        files_identical = original_data == collision_data

        return {
            'original_file': original_file,
            'collision_file': collision_file,
            'original_hash': original_hash,
            'collision_hash': collision_hash,
            'hashes_match': original_hash == collision_hash,
            'files_identical': files_identical,
            'is_valid_collision': original_hash == collision_hash and not files_identical,
            'original_size': len(original_data),
            'collision_size': len(collision_data),
            'bit_length': bit_length
        }


if __name__ == "__main__":
    # Test collision maker
    print("Testing Collision Maker\n")

    test_file = "test_files/source_code.py"

    if os.path.exists(test_file):
        print(f"Creating collision for: {test_file}")
        print(f"Using 2-bit hash (easier to find collisions)\n")

        collision_file = CollisionMaker.create_collision(test_file, bit_length=2, max_attempts=1000)

        if collision_file:
            print(f"\nVerifying collision...")
            result = CollisionMaker.verify_collision(test_file, collision_file, bit_length=2)

            print(f"\nVerification Results:")
            print(f"  Original: {result['original_file']}")
            print(f"  Collision: {result['collision_file']}")
            print(f"  Original hash: {result['original_hash']}")
            print(f"  Collision hash: {result['collision_hash']}")
            print(f"  Hashes match: {result['hashes_match']}")
            print(f"  Files identical: {result['files_identical']}")
            print(f"  Valid collision: {result['is_valid_collision']}")
    else:
        print(f"Test file not found: {test_file}")
