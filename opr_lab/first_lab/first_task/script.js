let result = 1;
for (let i = 1; i <= 6; i++) {
    enter_number = prompt('Введіть число')
    if (7 > enter_number && enter_number > 0) {
        result *= enter_number
    }
}
alert(result)