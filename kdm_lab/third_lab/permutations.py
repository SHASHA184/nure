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
            res = perm[:i] + elements[0:1] + perm[i:]
            yield res

n = int(input("Enter a number of set: "))
for p in permutation(list(range(n))):
    count += 1
    result.append(p)

end = time.time() - start

print(end)
if result:
    print('Result', '\n'.join(str(x) for x in result), f'\n{len(result)} - count of permutations')
else:
    print('Result', '\n'.join(str(x) for x in result), f'\n0 - count of permutations')
    