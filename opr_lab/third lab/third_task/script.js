let c = +prompt('Введіть число c')
let d = +prompt('Введіть число d')
let q = +prompt('Введіть число q')
let r = +prompt('Введіть число r')
let b = +prompt('Введіть число b')

console.log(a(+prompt('Введіть n')))

function a(k) {
    if (k == 0) {
        return c
    }
    else if (k == 1) {
        return d
    }
    return (q * a(k - 1) + r * a(k - 2) + b)
}