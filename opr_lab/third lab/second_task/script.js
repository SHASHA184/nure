
function binary_sequence(n) {
    let remainder = 0, sum = "", num_sum = [], rounded
    for (number of String(n)) {
        for (let i, temp = +number; temp != 0;temp = i) {
            if (temp in [0, 1]) {
                num_sum.push(temp)
                break
            }
            rounded = Math.ceil(temp / 2)
            i = parseInt(temp / 2)
            remainder = temp % rounded
            num_sum.push(remainder == 0 ? 0 : 1)
        }
        if (num_sum.length < 4) {
            for (let j = 4 - num_sum.length; j > 0; j--) {
                num_sum.push(0)
            }
        }
        for (let j = num_sum.length; j > 0; j--) {
            cur_number = num_sum.pop()
            sum += String(cur_number)
        }
        num_sum = []
    }
    return sum
}
let enter_number = +prompt("Введіть число")
let res = binary_sequence(enter_number)