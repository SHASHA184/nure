/*
Даний числовий масив arr. Замінити у ньому всі входження числа a на число b. Для цього визначте функцію replace(arr, a, b), яка нічого не повертає, але змінює масив arr.
Наприклад,
var arr = [1,2,3,1,2,3]; replace(arr, 3, 4); arr is [1,2,4,1,2,4];

*/
//BEGIN
function replace(arr, a, b) {       
    for (let i = 0; i < arr.length; i++) {
        if (arr[i] == a) {
            arr.slice(i, b)
        }
    }
}
//END
let a = [1,2,3,1,2,3];
replace(a, 2, 4);
console.log(a);   // (6) [1, 4, 3, 1, 4, 3]
