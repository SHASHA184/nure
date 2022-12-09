

function count_of_array(array, find) {
    find_array = []

    
    for (elem of find) {
        if (elem != ',' && elem != ' ') {
            find_array.push({elem: 0})
        }
        console.log(find_array)
    }
    for (i of array) {
        for (j of find_array) {
            cur_count = j[i]
            if (typeof cur_count !== "undefined") {
                j[i] += 1
            }
        }
    }
    return find_array
}

array = [prompt("Введіть елементи масиву")]
find = prompt("Введіть шукані елементи")
alert(count_of_array(array, find))