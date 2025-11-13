# Лабораторна робота 3: Багатопоточність

## Мета роботи
Отримання студентами практичних навичок створення та реалізації інструментів паралелізму: горутин, каналів, конвеєрів, загальної пам'яті та загальних змінних.

## Теоретичні відомості

### 1. Помилки, паніки, відновлення

#### Тип error
```go
type error interface {
    Error() string
}

// Створення помилок
err := errors.New("emit macho dwarf: elf header corrupted")
err := fmt.Errorf("user %q (id %d) not found", name, id)
```

#### Кастомні типи помилок
```go
type PathError struct {
    Op   string
    Path string
    Err  error
}

func (e *PathError) Error() string {
    return e.Op + " " + e.Path + ": " + e.Err.Error()
}
```

#### panic і recover
```go
func main() {
    defer func() {
        if err := recover(); err != nil {
            fmt.Printf("panic: %s", err)
        }
    }()
    foo()
}

func foo() {
    panic(errors.New("error"))
}
```

**Важливо:** Кожна горутина має окремий стек. Panic у горутині завершить всю програму, якщо не обробити recover у цій горутині.

#### Stack trace
```go
// Вивести stack trace
debug.PrintStack()

// Отримати stack trace як []byte
buf := debug.Stack()

// Stack trace всіх горутин
buf := make([]byte, 1024)
runtime.Stack(buf, true)
```

### 2. Написання та імпорт пакетів

#### Структура пакета
```
└── $GOPATH
    └── src
        └── github.com
            └── gopherguides
                └── greet
                    └── greet.go
```

#### Приклад пакета
```go
// greet.go
package greet

import "fmt"

var Shark = "Sammy"

type Octopus struct {
    Name  string
    Color string
}

func (o Octopus) String() string {
    return fmt.Sprintf("The octopus's name is %q and is the color %s.",
                       o.Name, o.Color)
}

func Hello() {
    fmt.Println("Hello, World!")
}
```

#### Використання пакета
```go
// main.go
package main

import (
    "fmt"
    "github.com/gopherguides/greet"
)

func main() {
    greet.Hello()
    fmt.Println(greet.Shark)

    oct := greet.Octopus{
        Name:  "Jesse",
        Color: "orange",
    }
    fmt.Println(oct.String())
}
```

### 3. Горутини (Goroutines)

Горутини представляють паралельні операції, які можуть виконуватися незалежно.

```go
// Запуск горутини
go factorial(5)

// Горутина з анонімною функцією
go func(n int) {
    result := 1
    for j := 1; j <= n; j++ {
        result *= j
    }
    fmt.Println(n, "-", result)
}(i)
```

**Важливо:** Функція main не чекає завершення горутин. Використовуйте синхронізацію!

### 4. Канали (Channels)

Канали - інструменти комунікації між горутинами.

#### Небуферизовані канали
```go
intCh := make(chan int)

// Відправлення в канал (блокується до отримання)
intCh <- 5

// Отримання з каналу (блокується до отримання даних)
val := <-intCh
```

Приклад використання:
```go
func main() {
    intCh := make(chan int)

    go func() {
        fmt.Println("Goroutine starts")
        intCh <- 5  // блокування до отримання
    }()

    fmt.Println(<-intCh)  // отримання даних
    fmt.Println("The End")
}
```

#### Буферизовані канали
```go
ch := make(chan int, 3)  // канал з буфером на 3 елементи

ch <- 1
ch <- 2
ch <- 3
// четвертий запис заблокує горутину

fmt.Println(<-ch)  // 1
fmt.Println(<-ch)  // 2
```

#### Закриття каналів
```go
ch := make(chan int)

go func() {
    for i := 0; i < 5; i++ {
        ch <- i
    }
    close(ch)  // закриття каналу
}()

// Отримання до закриття
for val := range ch {
    fmt.Println(val)
}
```

#### Перевірка закриття каналу
```go
val, ok := <-ch
if ok {
    fmt.Println("Отримано:", val)
} else {
    fmt.Println("Канал закрито")
}
```

### 5. Select

Оператор select дозволяє чекати на кілька операцій з каналами.

```go
select {
case msg1 := <-ch1:
    fmt.Println("Отримано з ch1:", msg1)
case msg2 := <-ch2:
    fmt.Println("Отримано з ch2:", msg2)
case ch3 <- 5:
    fmt.Println("Надіслано в ch3")
default:
    fmt.Println("Жодна операція не готова")
}
```

#### Timeout з select
```go
select {
case res := <-ch:
    fmt.Println(res)
case <-time.After(time.Second * 2):
    fmt.Println("Timeout!")
}
```

### 6. Sync пакет

#### WaitGroup
```go
var wg sync.WaitGroup

for i := 0; i < 5; i++ {
    wg.Add(1)
    go func(n int) {
        defer wg.Done()
        fmt.Println(n)
    }(i)
}

wg.Wait()  // чекаємо завершення всіх горутин
```

#### Mutex
```go
var (
    counter int
    mu      sync.Mutex
)

func increment() {
    mu.Lock()
    counter++
    mu.Unlock()
}
```

#### RWMutex
```go
var (
    data   map[string]string
    rwMu   sync.RWMutex
)

// Читання
func read(key string) string {
    rwMu.RLock()
    defer rwMu.RUnlock()
    return data[key]
}

// Запис
func write(key, value string) {
    rwMu.Lock()
    defer rwMu.Unlock()
    data[key] = value
}
```

## Завдання до виконання

### Завдання варіанти будуть надані викладачем

Типові завдання включають:
1. Створення горутин для паралельної обробки даних
2. Використання каналів для комунікації між горутинами
3. Реалізація producer-consumer патерну
4. Використання select для обробки множинних каналів
5. Синхронізація горутин за допомогою sync.WaitGroup
6. Безпечний доступ до спільних даних за допомогою Mutex
7. Реалізація конвеєрів (pipelines)
8. Обробка timeout та context

## Приклади патернів

### Generator Pattern
```go
func generator(nums ...int) <-chan int {
    out := make(chan int)
    go func() {
        for _, n := range nums {
            out <- n
        }
        close(out)
    }()
    return out
}
```

### Pipeline Pattern
```go
func sq(in <-chan int) <-chan int {
    out := make(chan int)
    go func() {
        for n := range in {
            out <- n * n
        }
        close(out)
    }()
    return out
}

// Використання
ch := generator(2, 3, 4)
out := sq(ch)
for val := range out {
    fmt.Println(val)  // 4, 9, 16
}
```

### Fan-out, Fan-in Pattern
```go
func fanOut(ch <-chan int, n int) []<-chan int {
    channels := make([]<-chan int, n)
    for i := 0; i < n; i++ {
        channels[i] = worker(ch)
    }
    return channels
}

func fanIn(channels ...<-chan int) <-chan int {
    out := make(chan int)
    var wg sync.WaitGroup

    for _, ch := range channels {
        wg.Add(1)
        go func(c <-chan int) {
            defer wg.Done()
            for val := range c {
                out <- val
            }
        }(ch)
    }

    go func() {
        wg.Wait()
        close(out)
    }()

    return out
}
```

## Структура проекту
```
lab3/
├── README.md (цей файл)
├── docs/
│   └── Лабораторна робота 3.docx
└── tasks/
    ├── goroutines/
    ├── channels/
    ├── pipelines/
    └── sync/
```

## Корисні команди
```bash
# Запуск з race detector
go run -race main.go

# Встановлення GOMAXPROCS
GOMAXPROCS=4 go run main.go

# Профілювання
go test -cpuprofile cpu.prof
go test -memprofile mem.prof
```

## Поради щодо багатопоточності

1. **Уникайте data races** - використовуйте `-race` flag
2. **Завжди закривайте канали** у відправнику, не в отримувачі
3. **Використовуйте context** для передачі сигналів скасування
4. **Обмежуйте кількість горутин** - не створюйте мільйони горутин
5. **Перевіряйте закриття каналів** при отриманні
6. **Не передавайте змінні циклу** в горутини напряму
