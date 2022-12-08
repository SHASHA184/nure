number = 28165
for i in range(0, 10):
    while True:
        t = number % 10
        if t != 0:
            print(t)
            number = (number - t) // 10
        else:
            break