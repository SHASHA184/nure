n = prompt("Скільки кілометрів пробіг першого дня")
k = prompt("На скільки відсоткув збільшував пробіг")
m = prompt("Скільки повинен пробігти кілометрів")
let result = 1

for (n; n <= m; n += n * (k/100)) {
    console.log("n - " + n + "\nk - " + k + "\nm - " + m)
    result++
}
alert(result)