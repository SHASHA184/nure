import re

expression = input('Enter your expression: ').replace(' ', '')
initials = "".join(re.split("[^a-zA-Z]*", expression))
all_initials = ''.join(sorted(set(initials)))
for char in all_initials:
    number = input(f'Please, enter number for {char}: ')
    expression = expression.replace(char, number)
for iteration, i in enumerate(expression):
    if i == '!':
        number = expression[iteration + 1]
        negation = expression[iteration] + number
        expression = expression.replace(negation, str(int(not int(number))))
operators = {'+': lambda x, y: x | y, '*': lambda x, y: x & y, '→': lambda x, y: int(not(x) or y), '≡': lambda x, y: int(x == y)}

operands = []
for i in expression:
    if i.isdigit():
        operands.append(int(i))
        print(f'Add {i} to operands\nResult: {operands}')
    elif i in operators and operands:
        try:
            first_value = operands.pop(-2)
            second_value = operands.pop(-1)
            new_value = operators[i](first_value, second_value)
            operands.append(new_value)
            print(" ".join(str(x) for x in [first_value, i, second_value]), '=', new_value)
        except IndexError:
            print("Enter in RPN")
            break
    
    

result = operands[0] if operands else None
print("Result of your expression: ", " ".join(expression), '=', result)