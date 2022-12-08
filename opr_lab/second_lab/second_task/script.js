
function check_on_natural(number) {
    if (number % 1 == 0 && number > 0) 
        return true
    else if (number % 1 != 0) {
        alert('Число потрібно бути цілим')
    }
    
    else if (number <= 0) {
        alert('Число потрібно бути додатнім')
    }
    
}

number = +prompt("Введіть натуральне число")
divider = +prompt("На яке число ділити")
check_on_natural(number)
min_number = 9
max_number = 0
for (let i; number != 0; number = parseInt((number - i) / 10)) {
    i = number % 10
    if (min_number > i) {
        min_number = i
    }
    if (max_number < i) {
        max_number = i
    }
        
}

divisible = min_number + max_number
alert(divisible, min_number, max_number)