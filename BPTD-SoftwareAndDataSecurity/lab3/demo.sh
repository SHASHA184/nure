#!/bin/bash
# Демонстраційний скрипт для лабораторної роботи №3

echo "======================================================================"
echo "  Лабораторна робота №3: Програмна реалізація хеш-функцій"
echo "======================================================================"
echo ""

echo "1. Обчислення хешів для всіх тестових файлів (2-біт):"
echo "----------------------------------------------------------------------"
python src/main.py hash-all --bits 2
echo ""

echo "2. Обчислення хешів для всіх тестових файлів (8-біт):"
echo "----------------------------------------------------------------------"
python src/main.py hash-all --bits 8
echo ""

echo "3. Тестування властивості перемішування (Avalanche Effect):"
echo "----------------------------------------------------------------------"
python src/main.py avalanche test_files/source_code.py --num-tests 100
echo ""

echo "4. Порівняння оригіналу та колізії (source_code.py):"
echo "----------------------------------------------------------------------"
python src/main.py compare test_files/source_code.py test_files/source_code_collision.py --bits 2
echo ""

echo "5. Порівняння оригіналу та колізії (document.docx):"
echo "----------------------------------------------------------------------"
python src/main.py compare test_files/document.docx test_files/document_collision.docx --bits 2
echo ""

echo "======================================================================"
echo "  Демонстрація завершена!"
echo "======================================================================"
echo ""
echo "Доступні команди:"
echo "  - python src/main.py hash <файл> --bits <2|4|8>"
echo "  - python src/main.py collision <файл> --bits <2|4|8>"
echo "  - python src/main.py avalanche <файл>"
echo "  - python src/main.py compare <файл1> <файл2>"
echo ""
