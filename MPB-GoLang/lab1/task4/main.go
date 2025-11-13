package main

import (
	"fmt"
)

// PrintArray виводить масив у форматованому вигляді
func PrintArray(arr []int, name string) {
	fmt.Printf("%s: ", name)
	if len(arr) == 0 {
		fmt.Println("(порожній масив)")
		return
	}
	fmt.Print("[")
	for i, val := range arr {
		if i > 0 {
			fmt.Print(", ")
		}
		fmt.Print(val)
	}
	fmt.Println("]")
}

// DeleteNElementsFromK видаляє N елементів починаючи з індексу K
func DeleteNElementsFromK(arr []int, k, n int) ([]int, error) {
	if k < 0 || k >= len(arr) {
		return arr, fmt.Errorf("індекс K (%d) виходить за межі масиву (розмір: %d)", k, len(arr))
	}

	if n <= 0 {
		return arr, fmt.Errorf("кількість елементів N (%d) повинна бути більше 0", n)
	}

	if k+n > len(arr) {
		n = len(arr) - k
		fmt.Printf("Увага: N скориговано до %d (щоб не виходити за межі масиву)\n", n)
	}

	result := make([]int, 0, len(arr)-n)
	result = append(result, arr[:k]...)
	result = append(result, arr[k+n:]...)

	return result, nil
}

// AddElementAtK додає елемент на позицію K
func AddElementAtK(arr []int, k, value int) ([]int, error) {
	if k < 0 || k > len(arr) {
		return arr, fmt.Errorf("індекс K (%d) виходить за межі масиву (розмір: %d)", k, len(arr))
	}

	result := make([]int, len(arr)+1)
	copy(result[:k], arr[:k])
	result[k] = value
	copy(result[k+1:], arr[k:])

	return result, nil
}

// RearrangePositiveNegative переміщує позитивні елементи на початок, негативні в кінець
func RearrangePositiveNegative(arr []int) []int {
	if len(arr) == 0 {
		return arr
	}

	result := make([]int, 0, len(arr))
	positives := make([]int, 0)
	negatives := make([]int, 0)
	zeros := make([]int, 0)

	for _, val := range arr {
		if val > 0 {
			positives = append(positives, val)
		} else if val < 0 {
			negatives = append(negatives, val)
		} else {
			zeros = append(zeros, val)
		}
	}

	result = append(result, positives...)
	result = append(result, zeros...)
	result = append(result, negatives...)

	return result
}

// FindFirstEven шукає перший парний елемент у масиві
func FindFirstEven(arr []int) (int, int, bool) {
	for i, val := range arr {
		if val%2 == 0 {
			return i, val, true
		}
	}
	return -1, 0, false
}

// ShowMenu виводить меню операцій
func ShowMenu() {
	fmt.Println("\n=== МЕНЮ ОПЕРАЦІЙ ===")
	fmt.Println("1. Видалення N елементів, починаючи з номера K")
	fmt.Println("2. Додавання елемента на позицію K")
	fmt.Println("3. Перестановка: позитивні на початок, негативні в кінець")
	fmt.Println("4. Пошук першого парного елемента")
	fmt.Println("5. Показати поточний масив")
	fmt.Println("6. Створити новий масив")
	fmt.Println("0. Вихід")
	fmt.Print("\nВиберіть операцію: ")
}

func main() {
	fmt.Println("Програма обробки одновимірного масиву")
	fmt.Println()

	var size int
	fmt.Print("Введіть розмір масиву: ")
	_, err := fmt.Scan(&size)
	if err != nil || size <= 0 {
		fmt.Println("Помилка: невірний розмір масиву")
		return
	}

	arr := make([]int, size)
	fmt.Printf("Введіть %d елементів масиву:\n", size)
	for i := 0; i < size; i++ {
		fmt.Printf("Елемент [%d]: ", i)
		_, err := fmt.Scan(&arr[i])
		if err != nil {
			fmt.Println("Помилка введення")
			return
		}
	}

	fmt.Println("\nПочатковий масив:")
	PrintArray(arr, "Масив")

	for {
		ShowMenu()

		var choice int
		_, err := fmt.Scan(&choice)
		if err != nil {
			fmt.Println("Помилка введення")
			continue
		}

		switch choice {
		case 1:
			fmt.Println("\n--- Видалення N елементів з позиції K ---")
			PrintArray(arr, "Поточний масив")

			var k, n int
			fmt.Print("Введіть початковий індекс K: ")
			fmt.Scan(&k)
			fmt.Print("Введіть кількість елементів N: ")
			fmt.Scan(&n)

			newArr, err := DeleteNElementsFromK(arr, k, n)
			if err != nil {
				fmt.Println("Помилка:", err)
			} else {
				arr = newArr
				fmt.Println("Результат:")
				PrintArray(arr, "Масив")
			}

		case 2:
			fmt.Println("\n--- Додавання елемента на позицію K ---")
			PrintArray(arr, "Поточний масив")

			var k, value int
			fmt.Print("Введіть індекс K: ")
			fmt.Scan(&k)
			fmt.Print("Введіть значення елемента: ")
			fmt.Scan(&value)

			newArr, err := AddElementAtK(arr, k, value)
			if err != nil {
				fmt.Println("Помилка:", err)
			} else {
				arr = newArr
				fmt.Println("Результат:")
				PrintArray(arr, "Масив")
			}

		case 3:
			fmt.Println("\n--- Перестановка: позитивні на початок, негативні в кінець ---")
			PrintArray(arr, "До перестановки")

			arr = RearrangePositiveNegative(arr)

			PrintArray(arr, "Після перестановки")

		case 4:
			fmt.Println("\n--- Пошук першого парного елемента ---")
			PrintArray(arr, "Масив")

			index, value, found := FindFirstEven(arr)
			if found {
				fmt.Printf("Знайдено: перший парний елемент = %d на позиції %d\n", value, index)
			} else {
				fmt.Println("Парних елементів не знайдено")
			}

		case 5:
			fmt.Println("\n--- Поточний стан масиву ---")
			PrintArray(arr, "Масив")
			fmt.Printf("Розмір: %d елементів\n", len(arr))

		case 6:
			fmt.Print("\nВведіть новий розмір масиву: ")
			_, err := fmt.Scan(&size)
			if err != nil || size <= 0 {
				fmt.Println("Помилка: невірний розмір масиву")
				continue
			}

			arr = make([]int, size)
			fmt.Printf("Введіть %d елементів масиву:\n", size)
			for i := 0; i < size; i++ {
				fmt.Printf("Елемент [%d]: ", i)
				fmt.Scan(&arr[i])
			}
			PrintArray(arr, "Новий масив")

		case 0:
			fmt.Println("\nЗавершення програми...")
			return

		default:
			fmt.Println("Невірний вибір. Спробуйте ще раз.")
		}
	}
}
