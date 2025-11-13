package main

import (
	"fmt"
)

// FunctionValue обчислює значення функції двох змінних
// y = a² + x, якщо a > x
// y = a²,     якщо a = x
// y = a² - x, якщо a < x
func FunctionValue(x, a float64) float64 {
	aSq := a * a
	if a > x {
		return aSq + x
	} else if a == x {
		return aSq
	} else {
		return aSq - x
	}
}

// TabulateFunction створює таблицю значень функції для всіх комбінацій x та a
func TabulateFunction(xStart, xEnd, xStep, aStart, aEnd, aStep float64) {
	// Створення масиву значень a для заголовка
	aValues := make([]float64, 0)
	for a := aStart; a <= aEnd+0.001; a += aStep {
		aValues = append(aValues, a)
	}

	// Друк заголовка таблиці
	fmt.Printf("%8s", "x\\a")
	for _, a := range aValues {
		fmt.Printf("%10.1f", a)
	}
	fmt.Println()
	fmt.Println()

	// Друк рядків таблиці
	for x := xStart; x <= xEnd+0.001; x += xStep {
		fmt.Printf("%8.1f", x)
		for _, a := range aValues {
			y := FunctionValue(x, a)
			fmt.Printf("%10.3f", y)
		}
		fmt.Println()
	}
}

func main() {
	fmt.Println("Табулювання функції двох змінних:")
	fmt.Println("y = a² + x, якщо a > x")
	fmt.Println("y = a²,     якщо a = x")
	fmt.Println("y = a² - x, якщо a < x")
	fmt.Println()

	var xStart, xEnd, xStep, aStart, aEnd, aStep float64

	fmt.Println("Введіть параметри для x:")
	fmt.Print("  Початкове значення: ")
	fmt.Scan(&xStart)
	fmt.Print("  Кінцеве значення: ")
	fmt.Scan(&xEnd)
	fmt.Print("  Крок: ")
	fmt.Scan(&xStep)

	fmt.Println("\nВведіть параметри для a:")
	fmt.Print("  Початкове значення: ")
	fmt.Scan(&aStart)
	fmt.Print("  Кінцеве значення: ")
	fmt.Scan(&aEnd)
	fmt.Print("  Крок: ")
	fmt.Scan(&aStep)

	fmt.Printf("\nПараметри табулювання:\n")
	fmt.Printf("x ∈ [%.1f, %.1f], крок = %.1f\n", xStart, xEnd, xStep)
	fmt.Printf("a ∈ [%.1f, %.1f], крок = %.1f\n\n", aStart, aEnd, aStep)

	TabulateFunction(xStart, xEnd, xStep, aStart, aEnd, aStep)
}
