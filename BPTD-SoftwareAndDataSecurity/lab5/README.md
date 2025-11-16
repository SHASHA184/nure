# Лабораторна робота №5 - Інтеграція платіжної системи

## Опис проекту

Інтернет-магазин автомобілів з інтеграцією платіжної системи Stripe в тестовому режимі.

### Функціонал

- ✅ **Інтеграція Stripe** у тестовому режимі для безпечних платежів
- ✅ **Автоматична обробка результатів оплати**:
  - При успішній оплаті: кількість товару зменшується (3 → 2)
  - При неуспішній оплаті: кількість залишається незмінною (3 → 3)
- ✅ **Webhook обробка** від Stripe для оновлення статусу замовлень в реальному часі
- ✅ **База даних SQLite** для зберігання автомобілів та замовлень
- ✅ **Responsive веб-інтерфейс**

### Оцінювання

Згідно з завданням:
- ❌ Задовільно: Простий донат
- ✅ **Добре**: Повноцінний сервіс з покупкою товарів + обробка результатів оплати
- ❌ Відмінно: + OAuth2 (не реалізовано)

### Структура проекту

```
lab5/
├── cmd/
│   └── server/
│       └── main.go              # Точка входу
├── internal/
│   ├── models/
│   │   └── models.go            # Моделі даних
│   ├── database/
│   │   └── database.go          # Робота з БД
│   ├── payment/
│   │   └── stripe.go            # Інтеграція Stripe
│   └── handlers/
│       └── handlers.go          # HTTP обробники
├── templates/
│   ├── index.html               # Головна сторінка
│   ├── success.html             # Успішна оплата
│   └── cancel.html              # Скасована оплата
├── docs/
│   └── ЛР №5.pdf               # Завдання
├── .env.example                 # Приклад конфігурації
├── .env                         # Конфігурація (заповнити!)
├── .gitignore
├── go.mod
└── README.md
```

## Налаштування

### 1. Встановити Go

Переконайтеся, що у вас встановлено Go версії 1.21 або новіше:

```bash
go version
```

### 2. Встановити залежності

```bash
go mod tidy
```

### 3. Налаштувати Stripe

#### 3.1 Створити акаунт Stripe

1. Перейти до [Stripe Dashboard](https://dashboard.stripe.com/)
2. Створити акаунт (безкоштовно)
3. Увімкнути **Test mode** (перемикач у верхньому правому куті)

#### 3.2 Отримати API ключі

1. Перейти до [Developers → API keys](https://dashboard.stripe.com/test/apikeys)
2. Скопіювати:
   - **Publishable key** (починається з `pk_test_`)
   - **Secret key** (кнопка "Reveal test key", починається з `sk_test_`)

#### 3.3 Налаштувати Webhook

Для локального тестування є два варіанти:

**Варіант 1: Stripe CLI (рекомендовано)**

```bash
# Встановити Stripe CLI
# https://stripe.com/docs/stripe-cli

# Увійти
stripe login

# Запустити webhook forwarding
stripe listen --forward-to localhost:8080/webhook/stripe

# Скопіювати webhook secret (починається з whsec_)
```

**Варіант 2: ngrok**

```bash
# Встановити ngrok
# https://ngrok.com/

# Запустити ngrok
ngrok http 8080

# Використати HTTPS URL для webhook endpoint
# Наприклад: https://abc123.ngrok.io/webhook/stripe
```

#### 3.4 Заповнити .env файл

```bash
# Відредагувати .env файл
nano .env
```

Вставити свої ключі:

```env
PORT=8080
HOST=localhost
DB_PATH=./car-shop.db

# Замінити на ваші ключі з Stripe Dashboard
STRIPE_SECRET_KEY=sk_test_51...
STRIPE_PUBLISHABLE_KEY=pk_test_51...
STRIPE_WEBHOOK_SECRET=whsec_...
```

## Запуск

```bash
# Запустити сервер
go run cmd/server/main.go

# Або скомпілювати та запустити
go build -o car-shop cmd/server/main.go
./car-shop
```

Сервер запуститься на `http://localhost:8080`

## Тестування платежів

### Тестові картки Stripe

**Успішна оплата:**
```
Номер картки: 4242 4242 4242 4242
Дата: будь-яка майбутня дата (наприклад, 12/34)
CVC: будь-які 3 цифри (наприклад, 123)
ZIP: будь-який (наприклад, 12345)
```

**Відхилена оплата (недостатньо коштів):**
```
Номер картки: 4000 0000 0000 9995
```

**Відхилена оплата (картка заблокована):**
```
Номер картки: 4000 0000 0000 0002
```

Повний список тестових карток: [Stripe Testing Documentation](https://stripe.com/docs/testing)

### Сценарій тестування

1. **Відкрити додаток** в браузері: `http://localhost:8080`

2. **Перевірити початковий стан:**
   - ЗАЗ 968: 3 шт в наявності
   - Chevrolet Lanos: 5 шт
   - ЗАЗ Tavria: 2 шт

3. **Тест успішної оплати:**
   - Натиснути "Купити зараз" на ЗАЗ 968
   - Відкриється Stripe Checkout
   - Ввести тестову картку `4242 4242 4242 4242`
   - Дата: `12/34`, CVC: `123`
   - Натиснути "Pay"
   - Перенаправлення на сторінку успіху
   - **Повернутися на головну** - кількість ЗАЗ 968 тепер **2 шт** ✓

4. **Тест скасованої оплати:**
   - Натиснути "Купити зараз" на Chevrolet Lanos
   - В Stripe Checkout натиснути "← Back" (стрілка назад)
   - Редірект на сторінку скасування
   - **Повернутися на головну** - кількість Lanos залишилась **5 шт** ✓

5. **Перевірка логів:**
   ```
   ✓ Payment successful for order 1, car stock updated: ЗАЗ 968 (now 2 in stock)
   ✗ Payment session expired for order 2 (stock unchanged)
   ```

## Як це працює

### Успішна оплата

1. Користувач натискає "Купити"
2. Backend створює Stripe Checkout Session
3. Створюється замовлення у БД зі статусом `pending`
4. Користувач перенаправляється на Stripe Checkout
5. Після успішної оплати Stripe надсилає webhook `checkout.session.completed`
6. Webhook обробник:
   - ✅ Оновлює статус замовлення на `paid`
   - ✅ **Зменшує кількість товару** (stock - 1)
   - ✅ Логує успішну операцію

### Невдала оплата

1. Якщо користувач закриває вікно оплати або сесія закінчується
2. Stripe надсилає webhook `checkout.session.expired`
3. Webhook обробник:
   - ✅ Оновлює статус замовлення на `cancelled`
   - ✅ **Кількість товару залишається незмінною**
   - ✅ Логує скасування

## API Endpoints

### Публічні
- `GET /` - Головна сторінка з каталогом
- `GET /api/cars` - JSON список автомобілів

### Оплата
- `POST /api/checkout` - Створення сесії оплати
  ```json
  Request: {"car_id": 1}
  Response: {"session_id": "cs_...", "publishable_key": "pk_test_..."}
  ```

### Результати
- `GET /payment/success` - Сторінка успішної оплати
- `GET /payment/cancel` - Сторінка скасованої оплати

### Webhooks
- `POST /webhook/stripe` - Обробка подій від Stripe

## Технології

- **Backend**: Go 1.21+
- **Database**: SQLite
- **Payment**: Stripe API v81 (Test Mode)
- **Frontend**: HTML, CSS, JavaScript (Vanilla)
- **Libraries**:
  - `gorilla/mux` - HTTP router
  - `stripe-go/v81` - Stripe SDK
  - `go-sqlite3` - SQLite driver
  - `godotenv` - Environment variables

## Безпека

- ✅ Використання тестового режиму Stripe (без реальних грошей)
- ✅ Перевірка підпису Stripe webhooks
- ✅ Environment variables для конфіденційних даних
- ✅ Валідація даних на сервері
- ✅ Перевірка наявності товару перед створенням замовлення

## Troubleshooting

### Помилка "Failed to create checkout session"
- Перевірте, чи правильно вказані Stripe API keys в `.env`
- Переконайтесь, що використовуєте **test** keys (pk_test_ та sk_test_)

### Webhook не спрацьовує
- Для локального тестування використовуйте Stripe CLI або ngrok
- Перевірте webhook secret
- Переконайтесь, що endpoint доступний
- Перевірте логи сервера

### Товар не зменшується після оплати
- Перевірте, чи запущений webhook forwarding (Stripe CLI / ngrok)
- Перевірте логи сервера - повинно бути повідомлення про успішну оплату
- Оновіть сторінку в браузері (F5)

### База даних не створюється
- Перевірте права на запис в поточній директорії
- Перевірте шлях до БД в `.env` файлі

## Корисні посилання

- [Stripe Dashboard](https://dashboard.stripe.com/)
- [Stripe Testing](https://stripe.com/docs/testing)
- [Stripe CLI](https://stripe.com/docs/stripe-cli)
- [Stripe Webhooks](https://stripe.com/docs/webhooks)
- [ngrok](https://ngrok.com/)

## Автор

Лабораторна робота №5 з курсу "Software and Data Security"
НУРЕ, 2025

**Оцінка:** Добре (повноцінний магазин з обробкою платежів)
