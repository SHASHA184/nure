class Main:
    def __init__(self, a, b, c):
        self.a, self.b, self.c = [self.sort_nad_delete_repeat(list(map(int, value))) for value in [a, b, c]]
        self.u = self.sort_nad_delete_repeat(self.a + self.b + self.c)
        
    def sort_nad_delete_repeat(self, value):
        res = []
        for i in value:
            if i not in res:
                res.append(i)
        return sorted(res)
    
    def union(self, first, second):
        return self.sort_nad_delete_repeat(first + second)

    def intersection(self, first, second):
        intersection_set = []
        for value in first:
            if value in second:
                intersection_set.append(value)
        return intersection_set

    def difference(self, first, second):
        all_numbers = first + second
        difference_set = []
        for number in all_numbers:
            if all_numbers.count(number) == 1:
                difference_set.append(number)
        
        return sorted(difference_set)
    
    def complement(self, set):
        complement_set = []
        for number in self.u:
            if number not in set:
                complement_set.append(number)
        return(complement_set)
    

while True:
    i = int(input("""
0. Exit
1. Create sets
2. Operations with sets
Enter number: """))
    if i == 0:
        print("Exit")
        break
    elif i == 1:
        a = input("\nEnter set a (write through coma): ").split(", ")
        b = input("Enter set b (write through coma): ").split(", ")
        c = input("Enter set c (write through coma): ").split(", ")
        main = Main(a, b, c)
        a, b, c, u = [main.a, main.b, main.c, main.u]
        print(f"\na - {a}, b - {b}, c - {c}, universal set u - {u}")
    elif i == 2:
        operation = int(input("""
0. Back
1. Union
2. Intersection
3. Difference
4. Complement
Which operation you want: """))
        print('\n')
        if operation == 0:
            print("Home")
        elif operation == 1:
            first = input("Choose a set among a, b, c: ")
            second = input("Choose a set among a, b, c: ")
            print(f"{first} U {second} = {main.union(getattr(main, first), getattr(main, second))}")
        elif operation == 2:
            first = input("Choose a set among a, b, c: ")
            second = input("Choose a set among a, b, c: ")
            print(f"{first} ∩ {main.intersection(getattr(main, first), getattr(main, second))}")
        elif operation == 3:
            first = input("Choose a set among a, b, c: ")
            second = input("Choose a set among a, b, c: ")
            print(f"{first} - {second} = {main.difference(getattr(main, first), getattr(main, second))}")
        elif operation == 4:
            set_list = input("Choose a set among a, b, c: ")
            print(f"{set_list}' = {main.complement(getattr(main, set_list))}")