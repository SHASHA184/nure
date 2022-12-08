function recognice_word(word, sequence_array) {
    let sequence = sequence_array[0], missing_letter_word = '', replaced_letter = '', confused_letter_word = '', sequence_word = '', result = ''
    // const ended_sym = [' ', ',', '.']
    for (letter of sequence) {
        if (letter != ',' || letter != '.' || letter != ' ') {
            sequence_word += letter
        }
        else {
            for (let i = 0; i < word.length; i++) {
                for (let j = 0; j < word.length; j++) {
                    if (j != i) {
                        missing_letter_word += word[j]
                        if (j != i+1) {
                            replaced_letter += word[j]
                        }
                        if (j < sequence_word.length) {
                            confused_letter_word += sequence_word[j]
                        }
                        
                    }

                    else {
                        replaced_letter += word[i+1] + word[i]
                    }

                }
                if (missing_letter_word == sequence_word || missing_letter_word == confused_letter_word || replaced_letter == sequence_word) {
                    result += `${sequence_word} це ${word}\n`
                }
                missing_letter_word = ''
                confused_letter_word = ''
                replaced_letter = ''

            }
            
            sequence_word = ''
        }
    }
    return result
}

alert(recognice_word('слово', ['солво, слво сово лово, слоло.']))