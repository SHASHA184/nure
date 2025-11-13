# Швидкий старт - Лабораторна робота №3

## Встановлення

```bash
# 1. Встановити залежності
pip install -r requirements.txt

# 2. Переконатися що тестові файли створені
python create_test_files.py
```

## Основні команди

### 1. Показати хеші всіх файлів
```bash
python src/main.py hash-all --bits 2
python src/main.py hash-all --bits 4
python src/main.py hash-all --bits 8
```

### 2. Тест avalanche effect
```bash
python src/main.py avalanche test_files/source_code.py --num-tests 200
```

### 3. Створити колізію
```bash
python src/main.py collision test_files/source_code.py --bits 2 --verify
```

### 4. Порівняти оригінал та колізію
```bash
python src/main.py compare test_files/source_code.py test_files/source_code_collision.py --bits 2
```

### 5. Запустити повну демонстрацію
```bash
./demo.sh
```

## Структура проекту

- `src/main.py` - головний CLI інтерфейс
- `src/hash_function.py` - власна хеш-функція
- `src/file_processor.py` - обробка файлів
- `src/collision_maker.py` - генерація колізій
- `test_files/` - тестові файли та колізії
- `README.md` - повна документація
- `REPORT.md` - звіт про виконану роботу

## Довідка

Для отримання допомоги по будь-якій команді:
```bash
python src/main.py --help
python src/main.py hash --help
python src/main.py collision --help
```

## Результати

Всі колізії вже створені та знаходяться в `test_files/`:
- `source_code_collision.py` - колізія для Python коду
- `document_collision.docx` - колізія для Word документу
- `image_collision.png` - колізія для зображення

Всі файли мають різний вміст, але однаковий 2-бітний хеш з оригіналом!
