let q = +prompt('Введіть число q')
let r = +prompt('Введіть число r')
let b = +prompt('Введіть число b')
let c = +prompt('Введіть число c')
let d = +prompt('Введіть число d')

alert(x(+prompt('Введіть n')))

function x(k) {
    if (k == 0) {
        return c
    }
    else if (k == 1) {
        return d
    }
    return (q * x(k - 1) + r * x(k - 2) + b)
}