package main

import (
	"fmt"
)

// CheckPositiveSums перевіряє, чи є сума будь-яких двох із трьох чисел позитивною
func CheckPositiveSums(a, b, c float64) {
	fmt.Printf("\nДані числа: a = %.2f, b = %.2f, c = %.2f\n", a, b, c)
	fmt.Println("\nПеревірка сум:")

	hasPositiveSum := false

	sumAB := a + b
	if sumAB > 0 {
		fmt.Printf("✓ a + b = %.2f + %.2f = %.2f (позитивна)\n", a, b, sumAB)
		hasPositiveSum = true
	} else {
		fmt.Printf("✗ a + b = %.2f + %.2f = %.2f (не позитивна)\n", a, b, sumAB)
	}

	sumAC := a + c
	if sumAC > 0 {
		fmt.Printf("✓ a + c = %.2f + %.2f = %.2f (позитивна)\n", a, c, sumAC)
		hasPositiveSum = true
	} else {
		fmt.Printf("✗ a + c = %.2f + %.2f = %.2f (не позитивна)\n", a, c, sumAC)
	}

	sumBC := b + c
	if sumBC > 0 {
		fmt.Printf("✓ b + c = %.2f + %.2f = %.2f (позитивна)\n", b, c, sumBC)
		hasPositiveSum = true
	} else {
		fmt.Printf("✗ b + c = %.2f + %.2f = %.2f (не позитивна)\n", b, c, sumBC)
	}

	fmt.Println("\nВисновок:")
	if hasPositiveSum {
		fmt.Println("Є хоча б одна позитивна сума з двох чисел.")
	} else {
		fmt.Println("Жодна сума з двох чисел не є позитивною.")
	}
}

func main() {
	fmt.Println("Програма визначає, чи є сума якихось двох із трьох чисел позитивною")
	fmt.Println()

	var a, b, c float64

	fmt.Print("Введіть перше число (a): ")
	_, err := fmt.Scan(&a)
	if err != nil {
		fmt.Println("Помилка введення:", err)
		return
	}

	fmt.Print("Введіть друге число (b): ")
	_, err = fmt.Scan(&b)
	if err != nil {
		fmt.Println("Помилка введення:", err)
		return
	}

	fmt.Print("Введіть третє число (c): ")
	_, err = fmt.Scan(&c)
	if err != nil {
		fmt.Println("Помилка введення:", err)
		return
	}

	CheckPositiveSums(a, b, c)
}
