package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
)

// ContainsSubstring перевіряє, чи містить слово підрядок
func ContainsSubstring(word, substring string) bool {
	return strings.Contains(strings.ToLower(word), strings.ToLower(substring))
}

// FindWordsWithSubstring знаходить всі слова, що містять заданий підрядок
func FindWordsWithSubstring(text, substring string) []string {
	words := strings.Fields(text)
	result := make([]string, 0)

	for _, word := range words {
		if ContainsSubstring(word, substring) {
			result = append(result, word)
		}
	}

	return result
}

func main() {
	fmt.Println("Обробка рядка")
	fmt.Println("Завдання: надрукувати слова, до яких входить поєднання «на»")
	fmt.Println()

	fmt.Println("Введіть текст:")
	reader := bufio.NewReader(os.Stdin)
	text, err := reader.ReadString('\n')
	if err != nil {
		fmt.Println("Помилка читання:", err)
		return
	}

	text = strings.TrimSpace(text)

	if text == "" {
		fmt.Println("Помилка: текст порожній")
		return
	}

	fmt.Printf("\nВведений текст: %s\n", text)

	words := FindWordsWithSubstring(text, "на")

	fmt.Println("\n=== Результат ===")
	if len(words) == 0 {
		fmt.Println("Слів з поєднанням «на» не знайдено")
	} else {
		fmt.Printf("Знайдено слів з «на»: %d\n", len(words))
		fmt.Println("\nСлова:")
		for i, word := range words {
			fmt.Printf("  %d. %s\n", i+1, word)
		}
	}
}
