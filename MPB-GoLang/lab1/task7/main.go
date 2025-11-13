package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
	"unicode"
)

// Date структура для зберігання дати
type Date struct {
	Day   int
	Month string
	Year  int
}

// IsWinterMonth перевіряє, чи є місяць зимовим
func IsWinterMonth(month string) bool {
	month = strings.ToLower(strings.TrimSpace(month))
	winterMonths := []string{"грудень", "січень", "лютий", "december", "january", "february"}

	for _, wm := range winterMonths {
		if month == wm {
			return true
		}
	}
	return false
}

// PrintDate виводить дату у форматованому вигляді
func PrintDate(d Date, index int) {
	fmt.Printf("  %d. %02d %s %d", index, d.Day, d.Month, d.Year)
	if IsWinterMonth(d.Month) {
		fmt.Print(" [зимовий місяць]")
	}
	fmt.Println()
}

// FindWinterDates знаходить дати із зимовими місяцями
func FindWinterDates(dates []Date) []Date {
	result := make([]Date, 0)
	for _, date := range dates {
		if IsWinterMonth(date.Month) {
			result = append(result, date)
		}
	}
	return result
}

// ContainsDigit перевіряє, чи містить рядок цифру
func ContainsDigit(s string) bool {
	for _, ch := range s {
		if unicode.IsDigit(ch) {
			return true
		}
	}
	return false
}

// RemoveStringsWithDigits видаляє всі рядки, в яких є хоча б одна цифра
func RemoveStringsWithDigits(lines []string) []string {
	result := make([]string, 0)
	for _, line := range lines {
		if !ContainsDigit(line) {
			result = append(result, line)
		}
	}
	return result
}

func main() {
	fmt.Println("Обробка масиву структур та рядків")
	fmt.Println()

	// Частина 1: Робота з датами
	fmt.Println("=== Частина 1: Обробка масиву дат ===")
	var n int
	fmt.Print("Введіть кількість дат: ")
	fmt.Scan(&n)

	if n <= 0 {
		fmt.Println("Помилка: кількість дат повинна бути більше 0")
		return
	}

	dates := make([]Date, n)

	for i := 0; i < n; i++ {
		fmt.Printf("\nДата %d:\n", i+1)
		fmt.Print("  День: ")
		fmt.Scan(&dates[i].Day)

		fmt.Print("  Місяць: ")
		fmt.Scan(&dates[i].Month)

		fmt.Print("  Рік: ")
		fmt.Scan(&dates[i].Year)
	}

	fmt.Println("\n=== Всі введені дати ===")
	for i, date := range dates {
		PrintDate(date, i+1)
	}

	winterDates := FindWinterDates(dates)
	fmt.Printf("\n=== Дати із зимовими місяцями ===\n")
	if len(winterDates) == 0 {
		fmt.Println("Дат із зимовими місяцями не знайдено")
	} else {
		fmt.Printf("Знайдено: %d дат(и)\n", len(winterDates))
		for i, date := range winterDates {
			PrintDate(date, i+1)
		}
	}

	// Частина 2: Робота з рядками
	fmt.Println("\n\n=== Частина 2: Обробка масиву рядків ===")
	var m int
	fmt.Print("Введіть кількість рядків: ")
	fmt.Scan(&m)

	if m <= 0 {
		fmt.Println("Помилка: кількість рядків повинна бути більше 0")
		return
	}

	lines := make([]string, m)

	fmt.Println("Введіть рядки (кожен на окремому рядку):")
	scanner := bufio.NewScanner(os.Stdin)
	for i := 0; i < m && scanner.Scan(); i++ {
		lines[i] = scanner.Text()
	}

	fmt.Println("\n=== Введені рядки ===")
	for i, line := range lines {
		hasDigit := ContainsDigit(line)
		fmt.Printf("  %d. \"%s\"", i+1, line)
		if hasDigit {
			fmt.Print(" [містить цифру]")
		}
		fmt.Println()
	}

	filtered := RemoveStringsWithDigits(lines)
	fmt.Println("\n=== Рядки після видалення тих, що містять цифри ===")
	if len(filtered) == 0 {
		fmt.Println("Всі рядки містили цифри і були видалені")
	} else {
		fmt.Printf("Залишилось рядків: %d\n", len(filtered))
		for i, line := range filtered {
			fmt.Printf("  %d. \"%s\"\n", i+1, line)
		}
	}
}
