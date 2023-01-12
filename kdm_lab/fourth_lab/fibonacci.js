count = +prompt("Enter count of fibonacci")


function fibonacci_recursive(n) {
    if (n <= 1) {
        return n
    }
    return fibonacci_recursive(n-1) + fibonacci_recursive(n-2)
}
console.time();
console.log(fibonacci_recursive(count))
console.timeEnd();


function fibonacci_iterative(n) {
    let a = 0, b = 1, temp
    for (let i = 1; i <= n; i++) {
        temp = a
        a += b
        b = temp
    }
    return a
}
console.time();
console.log(fibonacci_iterative(count))
console.timeEnd();



function fibonacci_closed(n) {
    const phi = (1 + Math.sqrt(5)) / 2
    return Math.round((Math.pow(phi, n) - Math.pow(1 - phi, n)) / Math.sqrt(5))
}

console.time();
console.log(fibonacci_closed(count))
console.timeEnd();
