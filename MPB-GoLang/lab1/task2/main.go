package main

import (
	"fmt"
	"math"
)

// CountDigits підраховує кількість цифр у натуральному числі
func CountDigits(number int) int {
	if number == 0 {
		return 1
	}

	if number < 0 {
		number = -number
	}

	return int(math.Floor(math.Log10(float64(number)))) + 1
}

// CountDigitsWithMap підраховує цифри використовуючи мапи (позиція -> значення цифри)
func CountDigitsWithMap(number int) (int, map[int]int) {
	if number < 0 {
		number = -number
	}

	digitMap := make(map[int]int)
	position := 0

	if number == 0 {
		digitMap[0] = 0
		return 1, digitMap
	}

	temp := number
	for temp > 0 {
		digit := temp % 10
		digitMap[position] = digit
		position++
		temp /= 10
	}

	return position, digitMap
}

// GetDigitCountDescription повертає опис на основі кількості цифр
func GetDigitCountDescription(count int) string {
	descriptions := map[int]string{
		1:  "одноцифрове число",
		2:  "двоцифрове число",
		3:  "трицифрове число",
		4:  "чотирицифрове число",
		5:  "п'ятицифрове число",
		6:  "шестицифрове число",
		7:  "семицифрове число",
		8:  "восьмицифрове число",
		9:  "дев'ятицифрове число",
		10: "десятицифрове число",
	}

	if desc, exists := descriptions[count]; exists {
		return desc
	}
	return fmt.Sprintf("число з %d цифр", count)
}

func main() {
	fmt.Println("Програма визначає кількість цифр у натуральному числі")
	fmt.Println("(використовуючи мапи)")
	fmt.Println()

	var number int

	fmt.Print("Введіть натуральне число: ")
	_, err := fmt.Scan(&number)
	if err != nil {
		fmt.Println("Помилка введення:", err)
		return
	}

	digitCount, digitMap := CountDigitsWithMap(number)

	fmt.Printf("\n=== Результати для числа %d ===\n", number)
	fmt.Printf("Кількість цифр: %d\n", digitCount)
	fmt.Printf("Опис: %s\n", GetDigitCountDescription(digitCount))

	fmt.Println("\nКарта цифр (позиція -> цифра):")
	for i := digitCount - 1; i >= 0; i-- {
		fmt.Printf("  Позиція %d: %d\n", i, digitMap[i])
	}
}
