
import time

len_hanoi = int(input("Enter a number of hanoi tower: "))

start = time.time()
def tower_of_hanoi(n, source, helper, target):
    if n == 0:
        return
    tower_of_hanoi(n - 1, source, target, helper)
    target.append(source.pop())
    print(source, helper, target)
    tower_of_hanoi(n - 1, helper, source, target)


source = list(range(len_hanoi, 0, -1))
print(source)
target = []
helper = []
tower_of_hanoi(len(source), source, helper, target)
end = time.time() - start
print(end)