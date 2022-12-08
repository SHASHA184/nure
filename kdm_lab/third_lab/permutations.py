
import time 

start = time.time()
result = []
count = 0

def permutation(elements):
    if len(elements) <= 1:
        yield elements
        return
    for perm in permutation(elements[1:]):
        for i in range(len(elements)):
            yield perm[:i] + elements[0:1] + perm[i:]

for p in permutation(list(range(1, 11))):
    count += 1
    result.append(p)


end = time.time() - start

print(end)
print(count)