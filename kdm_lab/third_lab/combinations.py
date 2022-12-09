import time 

start = time.time()

def combination(enter_list, k):
     
    if k == 0:
        return [[]]
     
    l = []
    for i in range(0, len(enter_list)):
        m = enter_list[i]

        remain_list = enter_list[i + 1:]

        remain_list_combo = combination(remain_list, k - 1)
        for p in remain_list_combo:
            l.append([m, *p])
           
    return l


n = int(input("Enter a number of set: "))
k = int(input("Enter a k-kombinations of set: "))
result = combination(list(range(0, n)), k)

end = time.time() - start

print(end)
if result:
    print('Result', '\n'.join(str(x) for x in result), f'\n{len(result)} - count of combinations')
else:
    print('Result', '\n'.join(str(x) for x in result), f'\n0 - count of combinations')
    
