function exclude(array1, array2) {
    for (let value of array1) {
        if (array2.includes(value)) {
            array1.splice(array1.indexOf(value), 1);
        }
    }
    return [array1];
}
//END
let a = ['a','b','c','d']
exclude(a, ['b','d','e','f'])
alert(a);   // (2)  ['a','c']
a = [1, 2];
exclude(a, ['b', 2])
alert(a);   // (1)  [1]