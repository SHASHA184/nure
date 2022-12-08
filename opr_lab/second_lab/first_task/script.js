let years = 8
let area = 50
let productivity_of_one = 30
let productivity
let harvest = 0
let productivity_result = ''
let area_result = ''
let harvest_result = ''
for (let i = 1; i <= years; i++) {
    productivity = productivity_of_one * area
    harvest += productivity
    if (i >= 2 && i <= 7) {
        productivity_result += `За ${i} рік врожайність ${productivity_of_one.toFixed(2)} центнерів\n`
    }
    
    if (i >= 3 && i <= 8) {
        area_result += `За ${i} рік площа ${area.toFixed(2)} гектарів\n`
    }

    if (i == 5) {
        harvest_result = `За 5 років врожайність ${harvest.toFixed(2)} центнерів\n`
    }

    area += area * 0.1
    productivity_of_one += productivity_of_one * 0.01
}
let result = productivity_result + area_result + harvest_result
alert(result)