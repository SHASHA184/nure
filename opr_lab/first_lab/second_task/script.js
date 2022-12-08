function sqrt (k, n) {
    if (n == 1) {
        return k ** 0.5
    }
    return Math.sqrt(k**n + sqrt(k, n - 1))
}

function check_on_natural(number) {
    if (number % 1 == 0 && number > 0) 
        return true
    else if (number % 1 != 0) {
        alert('Число має бути цілим')
    }
    
    else if (number <= 0) {
        alert('Число має бути додатнім')
    }
    
}

k = prompt('Введіть перше натуральне число')
n = prompt('Введіть друге натуральне число')

if (check_on_natural(n) && check_on_natural(k))
    alert("k - " + k + "\nn - " + n + "\nРезультат - " + sqrt(k, n))