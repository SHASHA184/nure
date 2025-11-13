#!/usr/bin/env python3
"""
Main CLI interface for Lab 3 - Hash Functions
Author: NURE Student
Course: Software and Data Security
"""

import argparse
import sys
import os

# Add src directory to path
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))

from hash_function import custom_hash, hash_to_binary_string, test_avalanche_effect
from file_processor import FileProcessor, process_directory
from collision_maker import CollisionMaker


def cmd_hash(args):
    """Calculate hash of a file"""
    try:
        hash_info = FileProcessor.calculate_hash(args.file, args.bits)

        print(f"\n{'='*60}")
        print(f"Hash Calculation Results ({args.bits}-bit)")
        print(f"{'='*60}")
        print(FileProcessor.format_hash_info(hash_info))
        print(f"{'='*60}\n")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


def cmd_hash_all(args):
    """Calculate hashes for all test files"""
    test_files = [
        "test_files/document.docx",
        "test_files/source_code.py",
        "test_files/image.png"
    ]

    print(f"\n{'='*70}")
    print(f"Calculating hashes for all test files ({args.bits}-bit)")
    print(f"{'='*70}\n")

    for file_path in test_files:
        if os.path.exists(file_path):
            try:
                hash_info = FileProcessor.calculate_hash(file_path, args.bits)
                print(FileProcessor.format_hash_info(hash_info))
                print(f"{'-'*70}\n")
            except Exception as e:
                print(f"Error processing {file_path}: {e}\n")
        else:
            print(f"File not found: {file_path}\n")


def cmd_compare(args):
    """Compare hashes of two files"""
    try:
        comparison = FileProcessor.compare_hashes(args.file1, args.file2, args.bits)

        print(f"\n{'='*60}")
        print(f"Hash Comparison ({args.bits}-bit)")
        print(f"{'='*60}\n")

        print("File 1:")
        print(FileProcessor.format_hash_info(comparison['file1']))
        print(f"\n{'-'*60}\n")

        print("File 2:")
        print(FileProcessor.format_hash_info(comparison['file2']))
        print(f"\n{'-'*60}\n")

        print("Comparison Results:")
        print(f"  Files identical: {comparison['files_identical']}")
        print(f"  Hashes match: {comparison['hashes_match']}")
        print(f"  Collision detected: {comparison['collision']}")

        if not comparison['hashes_match']:
            print(f"  Bits different: {comparison['bits_different']} / {args.bits}")
            percentage = (comparison['bits_different'] / args.bits) * 100
            print(f"  Difference: {percentage:.1f}%")

        print(f"{'='*60}\n")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


def cmd_collision(args):
    """Create a collision for a file"""
    try:
        print(f"\nCreating collision for: {args.file}")
        print(f"Using {args.bits}-bit hash")
        print(f"Max attempts: {args.max_attempts}\n")

        collision_file = CollisionMaker.create_collision(
            args.file,
            bit_length=args.bits,
            max_attempts=args.max_attempts
        )

        if collision_file:
            print(f"\n✓ Collision created successfully!")
            print(f"  Original: {args.file}")
            print(f"  Collision: {collision_file}")

            # Verify
            if args.verify:
                print(f"\nVerifying collision...")
                result = CollisionMaker.verify_collision(
                    args.file, collision_file, args.bits
                )

                print(f"\nVerification:")
                print(f"  Original hash: {result['original_hash']} "
                      f"({format(result['original_hash'], f'0{args.bits}b')})")
                print(f"  Collision hash: {result['collision_hash']} "
                      f"({format(result['collision_hash'], f'0{args.bits}b')})")
                print(f"  Valid collision: {'✓' if result['is_valid_collision'] else '✗'}")

        else:
            print(f"\n✗ Failed to create collision")
            sys.exit(1)

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


def cmd_avalanche(args):
    """Test avalanche effect"""
    try:
        # Read file
        with open(args.file, 'rb') as f:
            data = f.read()

        print(f"\nTesting avalanche effect for: {args.file}")
        print(f"File size: {len(data)} bytes")
        print(f"Number of tests: {args.num_tests}\n")

        for bit_length in [2, 4, 8]:
            percentage = test_avalanche_effect(data, bit_length, args.num_tests)
            status = "✓" if percentage >= 30 else "✗"
            print(f"{bit_length}-bit hash: {percentage:.2f}% bits change on average {status}")

        print(f"\n{'✓' if percentage >= 30 else '✗'} "
              f"Requirement: ≥30% bits should change")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


def cmd_process_dir(args):
    """Process all files in a directory"""
    try:
        results = process_directory(args.directory, args.bits)

        print(f"\n{'='*60}")
        print(f"Processing directory: {args.directory}")
        print(f"Hash length: {args.bits} bits")
        print(f"{'='*60}\n")

        for hash_info in results:
            print(FileProcessor.format_hash_info(hash_info))
            print(f"{'-'*60}\n")

        print(f"Total files processed: {len(results)}")

    except Exception as e:
        print(f"Error: {e}", file=sys.stderr)
        sys.exit(1)


def main():
    """Main entry point"""
    parser = argparse.ArgumentParser(
        description='Lab 3: Custom Hash Function Implementation',
        formatter_class=argparse.RawDescriptionHelpFormatter,
        epilog="""
Examples:
  # Calculate hash of a file
  python main.py hash test_files/document.docx --bits 8

  # Calculate hashes for all test files
  python main.py hash-all --bits 2

  # Compare two files
  python main.py compare test_files/document.docx test_files/document_collision.docx

  # Create a collision
  python main.py collision test_files/source_code.py --bits 2

  # Test avalanche effect
  python main.py avalanche test_files/image.png --num-tests 200
        """
    )

    subparsers = parser.add_subparsers(dest='command', help='Available commands')

    # Hash command
    parser_hash = subparsers.add_parser('hash', help='Calculate hash of a file')
    parser_hash.add_argument('file', help='File to hash')
    parser_hash.add_argument('--bits', type=int, choices=[2, 4, 8], default=8,
                            help='Hash length in bits (default: 8)')
    parser_hash.set_defaults(func=cmd_hash)

    # Hash all command
    parser_hash_all = subparsers.add_parser('hash-all',
                                            help='Calculate hashes for all test files')
    parser_hash_all.add_argument('--bits', type=int, choices=[2, 4, 8], default=8,
                                help='Hash length in bits (default: 8)')
    parser_hash_all.set_defaults(func=cmd_hash_all)

    # Compare command
    parser_compare = subparsers.add_parser('compare', help='Compare hashes of two files')
    parser_compare.add_argument('file1', help='First file')
    parser_compare.add_argument('file2', help='Second file')
    parser_compare.add_argument('--bits', type=int, choices=[2, 4, 8], default=8,
                               help='Hash length in bits (default: 8)')
    parser_compare.set_defaults(func=cmd_compare)

    # Collision command
    parser_collision = subparsers.add_parser('collision',
                                            help='Create a collision for a file')
    parser_collision.add_argument('file', help='File to create collision for')
    parser_collision.add_argument('--bits', type=int, choices=[2, 4, 8], default=2,
                                 help='Hash length in bits (default: 2)')
    parser_collision.add_argument('--max-attempts', type=int, default=10000,
                                 help='Maximum attempts (default: 10000)')
    parser_collision.add_argument('--verify', action='store_true',
                                 help='Verify collision after creation')
    parser_collision.set_defaults(func=cmd_collision)

    # Avalanche command
    parser_avalanche = subparsers.add_parser('avalanche',
                                            help='Test avalanche effect')
    parser_avalanche.add_argument('file', help='File to test')
    parser_avalanche.add_argument('--num-tests', type=int, default=100,
                                 help='Number of tests to run (default: 100)')
    parser_avalanche.set_defaults(func=cmd_avalanche)

    # Process directory command
    parser_dir = subparsers.add_parser('process-dir',
                                       help='Process all files in directory')
    parser_dir.add_argument('directory', help='Directory to process')
    parser_dir.add_argument('--bits', type=int, choices=[2, 4, 8], default=8,
                           help='Hash length in bits (default: 8)')
    parser_dir.set_defaults(func=cmd_process_dir)

    # Parse arguments
    args = parser.parse_args()

    if args.command is None:
        parser.print_help()
        sys.exit(1)

    # Execute command
    args.func(args)


if __name__ == "__main__":
    main()
