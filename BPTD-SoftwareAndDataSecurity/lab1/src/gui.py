"""
GUI для DES
"""

import tkinter as tk
from tkinter import ttk, scrolledtext, messagebox
from matplotlib.backends.backend_tkagg import FigureCanvasTkAgg
from matplotlib.figure import Figure
from des import DES


class DESApp:
    def __init__(self, root):
        self.root = root
        self.root.title("DES Шифрування")
        self.root.geometry("1000x800")

        self.last_entropies = []
        self.des_instance = None

        self._create_widgets()

    def _create_widgets(self):
        # Головний контейнер
        main_frame = ttk.Frame(self.root, padding="10")
        main_frame.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))

        # Налаштування розмірів
        self.root.columnconfigure(0, weight=1)
        self.root.rowconfigure(0, weight=1)
        main_frame.columnconfigure(0, weight=1)

        # Заголовок
        title_label = ttk.Label(
            main_frame,
            text="DES Шифрування",
            font=('Arial', 16, 'bold')
        )
        title_label.grid(row=0, column=0, pady=(0, 10))

        # Notebook для вкладок
        self.notebook = ttk.Notebook(main_frame)
        self.notebook.grid(row=1, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        main_frame.rowconfigure(1, weight=1)

        # Вкладка шифрування/розшифрування
        self.crypto_frame = ttk.Frame(self.notebook, padding="10")
        self.notebook.add(self.crypto_frame, text="Шифрування/Розшифрування")

        # Вкладка ентропії
        self.entropy_frame = ttk.Frame(self.notebook, padding="10")
        self.notebook.add(self.entropy_frame, text="Аналіз ентропії")

        # Заповнення вкладок
        self._create_crypto_tab()
        self._create_entropy_tab()

    def _create_crypto_tab(self):
        """Створення вкладки шифрування/розшифрування"""

        # Поле для ключа
        key_frame = ttk.LabelFrame(self.crypto_frame, text="Ключ (8 символів)", padding="10")
        key_frame.grid(row=0, column=0, sticky=(tk.W, tk.E), pady=5)
        self.crypto_frame.columnconfigure(0, weight=1)

        self.key_entry = ttk.Entry(key_frame, width=40, font=('Courier', 10))
        self.key_entry.grid(row=0, column=0, sticky=(tk.W, tk.E))
        self.key_entry.insert(0, "DESCRYPT")
        key_frame.columnconfigure(0, weight=1)

        # Кнопка перевірки ключа
        check_key_btn = ttk.Button(
            key_frame,
            text="Перевірити ключ",
            command=self._check_key
        )
        check_key_btn.grid(row=0, column=1, padx=(10, 0))

        # Поле для вхідного тексту
        input_frame = ttk.LabelFrame(self.crypto_frame, text="Вхідний текст", padding="10")
        input_frame.grid(row=1, column=0, sticky=(tk.W, tk.E, tk.N, tk.S), pady=5)
        self.crypto_frame.rowconfigure(1, weight=1)

        self.input_text = scrolledtext.ScrolledText(
            input_frame,
            height=8,
            font=('Courier', 10),
            wrap=tk.WORD
        )
        self.input_text.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        self.input_text.insert(1.0, "Hello, DES!")
        input_frame.columnconfigure(0, weight=1)
        input_frame.rowconfigure(0, weight=1)

        # Кнопки
        buttons_frame = ttk.Frame(self.crypto_frame)
        buttons_frame.grid(row=2, column=0, pady=10)

        encrypt_btn = ttk.Button(
            buttons_frame,
            text="Зашифрувати",
            command=self._encrypt,
            width=20
        )
        encrypt_btn.grid(row=0, column=0, padx=5)

        decrypt_btn = ttk.Button(
            buttons_frame,
            text="Розшифрувати",
            command=self._decrypt,
            width=20
        )
        decrypt_btn.grid(row=0, column=1, padx=5)

        clear_btn = ttk.Button(
            buttons_frame,
            text="Очистити",
            command=self._clear_fields,
            width=20
        )
        clear_btn.grid(row=0, column=2, padx=5)

        # Поле для вихідного тексту
        output_frame = ttk.LabelFrame(self.crypto_frame, text="Результат", padding="10")
        output_frame.grid(row=3, column=0, sticky=(tk.W, tk.E, tk.N, tk.S), pady=5)
        self.crypto_frame.rowconfigure(3, weight=1)

        self.output_text = scrolledtext.ScrolledText(
            output_frame,
            height=8,
            font=('Courier', 10),
            wrap=tk.WORD,
            state=tk.DISABLED
        )
        self.output_text.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        output_frame.columnconfigure(0, weight=1)
        output_frame.rowconfigure(0, weight=1)

    def _create_entropy_tab(self):
        """Створення вкладки аналізу ентропії"""

        # Фрейм для графіка
        self.figure = Figure(figsize=(8, 6), dpi=100)
        self.ax = self.figure.add_subplot(111)

        self.canvas = FigureCanvasTkAgg(self.figure, self.entropy_frame)
        self.canvas.get_tk_widget().grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))

        self.entropy_frame.columnconfigure(0, weight=1)
        self.entropy_frame.rowconfigure(0, weight=1)

        # Початковий порожній графік
        self._plot_entropy([])

        # Таблиця з даними
        table_frame = ttk.LabelFrame(self.entropy_frame, text="Детальна інформація", padding="10")
        table_frame.grid(row=1, column=0, sticky=(tk.W, tk.E), pady=10)

        # Створення таблиці
        columns = ('Раунд', 'Ентропія', 'Одиниць', 'Нулів', 'Ймовірність 1')
        self.entropy_table = ttk.Treeview(table_frame, columns=columns, show='headings', height=6)

        for col in columns:
            self.entropy_table.heading(col, text=col)
            self.entropy_table.column(col, width=100, anchor=tk.CENTER)

        scrollbar = ttk.Scrollbar(table_frame, orient=tk.VERTICAL, command=self.entropy_table.yview)
        self.entropy_table.configure(yscroll=scrollbar.set)

        self.entropy_table.grid(row=0, column=0, sticky=(tk.W, tk.E, tk.N, tk.S))
        scrollbar.grid(row=0, column=1, sticky=(tk.N, tk.S))

        table_frame.columnconfigure(0, weight=1)


    def _check_key(self):
        """Перевірка ключа на слабкість"""
        key_str = self.key_entry.get()

        if len(key_str) != 8:
            messagebox.showwarning(
                "Попередження",
                "Ключ повинен складатися рівно з 8 символів!"
            )
            return

        key_bytes = key_str.encode('utf-8')

        if DES.is_weak_key(key_bytes):
            messagebox.showerror(
                "Слабкий ключ!",
                "Цей ключ є слабким та небезпечним.\nВиберіть інший ключ."
            )
        else:
            messagebox.showinfo(
                "Перевірка пройдена",
                "Ключ коректний."
            )

    def _encrypt(self):
        """Шифрування тексту"""
        try:
            # Отримання ключа
            key_str = self.key_entry.get()
            if len(key_str) != 8:
                messagebox.showerror("Помилка", "Ключ повинен бути 8 символів!")
                return

            key_bytes = key_str.encode('utf-8')

            # Отримання тексту
            plaintext = self.input_text.get(1.0, tk.END).strip()
            if not plaintext:
                messagebox.showerror("Помилка", "Введіть текст для шифрування!")
                return

            plaintext_bytes = plaintext.encode('utf-8')

            # Шифрування
            self.des_instance = DES(key_bytes)
            ciphertext = self.des_instance.encrypt(plaintext_bytes)

            # Збереження ентропій
            self.last_entropies = self.des_instance.get_last_round_entropies()

            # Відображення результату
            self.output_text.config(state=tk.NORMAL)
            self.output_text.delete(1.0, tk.END)
            self.output_text.insert(1.0, ciphertext.hex())
            self.output_text.config(state=tk.DISABLED)

            # Оновлення графіка ентропії
            self._plot_entropy(self.last_entropies)
            self._update_entropy_table(self.last_entropies)

        except ValueError as e:
            messagebox.showerror("Помилка", str(e))
        except Exception as e:
            messagebox.showerror("Помилка", f"Виникла помилка: {str(e)}")

    def _decrypt(self):
        """Розшифрування тексту"""
        try:
            # Отримання ключа
            key_str = self.key_entry.get()
            if len(key_str) != 8:
                messagebox.showerror("Помилка", "Ключ повинен бути 8 символів!")
                return

            key_bytes = key_str.encode('utf-8')

            # Отримання зашифрованого тексту
            ciphertext_hex = self.input_text.get(1.0, tk.END).strip()
            if not ciphertext_hex:
                messagebox.showerror("Помилка", "Введіть HEX-текст для розшифрування!")
                return

            # Видалення можливих пробілів
            ciphertext_hex = ciphertext_hex.replace(" ", "").replace("\n", "")

            try:
                ciphertext = bytes.fromhex(ciphertext_hex)
            except ValueError:
                messagebox.showerror("Помилка", "Невірний HEX формат!")
                return

            # Розшифрування
            des = DES(key_bytes)
            plaintext = des.decrypt(ciphertext)

            # Відображення результату
            self.output_text.config(state=tk.NORMAL)
            self.output_text.delete(1.0, tk.END)
            self.output_text.insert(1.0, plaintext.decode('utf-8'))
            self.output_text.config(state=tk.DISABLED)

        except ValueError as e:
            messagebox.showerror("Помилка", str(e))
        except Exception as e:
            messagebox.showerror("Помилка", f"Виникла помилка: {str(e)}")

    def _clear_fields(self):
        """Очищення полів"""
        self.input_text.delete(1.0, tk.END)
        self.output_text.config(state=tk.NORMAL)
        self.output_text.delete(1.0, tk.END)
        self.output_text.config(state=tk.DISABLED)

    def _plot_entropy(self, entropies):
        """Побудова графіка ентропії"""
        self.ax.clear()

        if not entropies:
            self.ax.text(
                0.5, 0.5,
                'Виконайте шифрування для відображення графіка',
                ha='center', va='center',
                fontsize=12
            )
            self.ax.set_xlim(0, 1)
            self.ax.set_ylim(0, 1)
        else:
            rounds = [e['round'] for e in entropies]
            entropy_values = [e['entropy'] for e in entropies]

            self.ax.plot(rounds, entropy_values, 'b-o', linewidth=2, markersize=6)
            self.ax.axhline(y=1.0, color='r', linestyle='--', linewidth=1, label='Максимальна ентропія')
            self.ax.set_xlabel('Номер раунду', fontsize=11)
            self.ax.set_ylabel('Ентропія (біти)', fontsize=11)
            self.ax.set_title('Ентропія появи біту 1 на кожному раунді', fontsize=12, fontweight='bold')
            self.ax.grid(True, alpha=0.3)
            self.ax.legend()
            self.ax.set_xlim(0, 17)
            self.ax.set_ylim(0, 1.1)

        self.canvas.draw()

    def _update_entropy_table(self, entropies):
        """Оновлення таблиці з даними ентропії"""
        # Очистка таблиці
        for item in self.entropy_table.get_children():
            self.entropy_table.delete(item)

        # Заповнення таблиці
        for e in entropies:
            prob_one = e['ones'] / (e['ones'] + e['zeros'])
            self.entropy_table.insert('', tk.END, values=(
                e['round'],
                f"{e['entropy']:.6f}",
                e['ones'],
                e['zeros'],
                f"{prob_one:.4f}"
            ))


def main():
    """Запуск програми"""
    root = tk.Tk()
    app = DESApp(root)
    root.mainloop()


if __name__ == "__main__":
    main()
