function train(t1, t2) {
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
