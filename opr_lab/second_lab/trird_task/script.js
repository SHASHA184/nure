function find2(number) {
    let all_numbers = 0
    for (let n = number; n != 0; n = parseInt((n - i) / 10)) {
        i = number % 10
        all_numbers += i
    }



    let max_number = 0
    let second_max_number = 0
    for (let i, n = number; n != 0; n = parseInt((n - i) / 10)) {
        i = n % 10

        if (max_number < i) {
            max_number = i
        }
    }

    console.log(max_number)

    for (let i, n = number; n != 0; n = parseInt((n - i) / 10)) {
        i = number % 10

        if (second_max_number < max_number) {
            second_max_number = i
        }
    }

    return second_max_number

}

find2(876)