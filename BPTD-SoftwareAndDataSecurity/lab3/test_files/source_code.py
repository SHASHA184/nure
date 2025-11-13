"""
Example Python source code file for testing hash function
"""


def fibonacci(n):
    """Calculate nth Fibonacci number"""
    if n <= 1:
        return n
    return fibonacci(n - 1) + fibonacci(n - 2)


def factorial(n):
    """Calculate factorial of n"""
    if n <= 1:
        return 1
    return n * factorial(n - 1)


def is_prime(n):
    """Check if number is prime"""
    if n < 2:
        return False
    for i in range(2, int(n ** 0.5) + 1):
        if n % i == 0:
            return False
    return True


if __name__ == "__main__":
    print("Testing mathematical functions:")
    print(f"fibonacci(10) = {fibonacci(10)}")
    print(f"factorial(5) = {factorial(5)}")
    print(f"is_prime(17) = {is_prime(17)}")
