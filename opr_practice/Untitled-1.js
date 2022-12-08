/*
Заданы две строки вида 'hh:mm', где hh:mm - время по 24-часовой системе, т.е. hh и mm обозначают часы и минуты соответственно.
Первая строка задает время начала движения поезда, а вторая - время окончания движения. 
Известно, что время поездки больше ноля, но меньше суток. Вычислить время движения поезда.
Объявить функцию train(t1,t2), которая возвращает время движения в виде строки 'hh:mm', например,
train('00:01','01:02') вернет строку '01:01'.
*/
//BEGIN
function train(t1, t2) {
    // let mins_per_hour = 60, mins_per_day = 1440
    // let first_array = t1.split(':')
    // let starthour = parseInt(first_array[0])
    // let startminute = parseInt(first_array[1])
    // let second_array = t2.split(':')
    // let endhour = parseInt(second_array[0])
    // let endminute = parseInt(second_array[1])
    // let startx = starthour * mins_per_hour + startminute
    // let endx = endhour * mins_per_hour + endminute

    // let duration = endx - startx
    // if (duration < 0)
    //     duration = duration + mins_per_day
    // durHours = parseInt(duration / 60)
    // durMinutes = duration % 60
    // if (durHours == 0) {
    //     durHours = '00'

    //     return durHours + ':' + String(durMinutes)
    // }
        
    // else {
    //     return String(durHours) + ':' + String(durMinutes)
    // }
        
    let mins_per_hour = 60, mins_per_day = 1440
    let first_array = t1.split(':')
    let starthour = parseInt(first_array[0])
    let startminute = parseInt(first_array[1])
    let second_array = t2.split(':')
    let endhour = parseInt(second_array[0])
    let endminute = parseInt(second_array[1])
    let startx = starthour * mins_per_hour + startminute
    let endx = endhour * mins_per_hour + endminute

    let duration = endx - startx
    if (duration < 0)
        duration = duration + mins_per_day
    let durHours = parseInt(duration / 60)
    let durMinutes = duration % 60
    let durHoursstr = String(durHours)
    let durMinutesstr = String(durMinutes)
    if (durHoursstr.length == 1) {
        durHoursstr = '0' + durHoursstr
    }
    
    if (durMinutesstr.length == 1) {
        durMinutesstr = '0' + durMinutesstr
    }



    return durHoursstr + ':' + durMinutesstr

}
//END

console.log(train('10:01','10:11'));             // 00:10
console.log(train('23:50','00:05'));             // 00:15
