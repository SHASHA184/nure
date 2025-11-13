package main

import (
	"fmt"
)

// PrintMatrix виводить двовимірний масив у форматованому вигляді
func PrintMatrix(matrix [][]int) {
	for i, row := range matrix {
		fmt.Printf("Рядок %d: ", i)
		for _, val := range row {
			fmt.Printf("%4d ", val)
		}
		fmt.Println()
	}
}

// HasAllNonZero перевіряє, чи всі елементи рядка не нулі
func HasAllNonZero(row []int) bool {
	for _, val := range row {
		if val == 0 {
			return false
		}
	}
	return true
}

// CountPositiveEven підраховує кількість позитивних парних чисел у рядку
func CountPositiveEven(row []int) int {
	count := 0
	for _, val := range row {
		if val > 0 && val%2 == 0 {
			count++
		}
	}
	return count
}

// ProcessMatrix обробляє матрицю згідно з завданням
func ProcessMatrix(matrix [][]int) {
	fmt.Println("\n=== Аналіз рядків ===")

	foundRow := false

	for i, row := range matrix {
		hasNonZero := HasAllNonZero(row)
		fmt.Printf("\nРядок %d: ", i)
		for _, val := range row {
			fmt.Printf("%4d ", val)
		}

		if hasNonZero {
			count := CountPositiveEven(row)
			fmt.Printf("\n  ✓ Всі елементи не нулі")
			fmt.Printf("\n  Кількість позитивних парних чисел: %d", count)
			foundRow = true
		} else {
			fmt.Printf("\n  ✗ Є нульові елементи")
		}
		fmt.Println()
	}

	if !foundRow {
		fmt.Println("\nУ жодному рядку немає всіх ненульових елементів.")
	}
}

func main() {
	fmt.Println("Обробка двовимірного масиву")
	fmt.Println("Завдання: знайти кількість позитивних парних чисел рядка,")
	fmt.Println("          у якого всі елементи не нулі")
	fmt.Println()

	var rows, cols int

	fmt.Print("Введіть кількість рядків: ")
	_, err := fmt.Scan(&rows)
	if err != nil || rows <= 0 {
		fmt.Println("Помилка: невірна кількість рядків")
		return
	}

	fmt.Print("Введіть кількість стовпців: ")
	_, err = fmt.Scan(&cols)
	if err != nil || cols <= 0 {
		fmt.Println("Помилка: невірна кількість стовпців")
		return
	}

	matrix := make([][]int, rows)
	for i := range matrix {
		matrix[i] = make([]int, cols)
	}

	fmt.Printf("\nВведіть елементи матриці %dx%d:\n", rows, cols)
	for i := 0; i < rows; i++ {
		fmt.Printf("Рядок %d (%d елементів): ", i, cols)
		for j := 0; j < cols; j++ {
			_, err := fmt.Scan(&matrix[i][j])
			if err != nil {
				fmt.Println("Помилка введення")
				return
			}
		}
	}

	fmt.Println("\n=== Введена матриця ===")
	PrintMatrix(matrix)

	ProcessMatrix(matrix)
}
