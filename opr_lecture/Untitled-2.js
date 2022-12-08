/*
Оголосити функцію exclude(), яка отримує два масиви та
видаляє з першого масиву всі елементи, що зустрічаються у другому масиві,
наприклад, exclude(['a','b','c','d'], ['b','d','e','f']) => ['a','c' ].
У вихідних масивах немає елементів, що повторюються.

*/
//BEGIN
function exclude(array1, array2) {
    for (let value of array1) {
        console.log("value - ", value)
        if (value == array2.indexOf(value)) {
            array1.splice(array1.indexOf(value), 1);
        }
    }
    console.log("array1 - ", array1)
    return array1, array2;
}
//END
let a = ['a','b','c','d']
exclude(a, ['b','d','e','f'])
console.log(a);   // (2)  ['a','c']
a = [1, 2];
exclude(a, ['b', 2])
console.log(a);   // (1)  [1]
