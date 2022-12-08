
import time 

start = time.time()

def combination(enter_list, n):
     
    if n == 0:
        return [[]]
     
    l = []
    for i in range(0, len(enter_list)):
        m = enter_list[i]

        remain_list = enter_list[i + 1:]

        remain_list_combo = combination(remain_list, n - 1)
        for p in remain_list_combo:
             l.append([m, *p])
           
    return l


result = combination(list(range(1, 4)), 3)

end = time.time() - start

print(end)
print(result)
