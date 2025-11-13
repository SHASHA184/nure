# Лабораторна робота 4: Основи веб-програмування в GoLang

## Мета роботи
Отримання студентами практичних навичок веб-програмування в GoLang.

## Теоретичні відомості

### 1. Основний функціонал (net/http)

#### Простий веб-сервер
```go
package main

import (
    "fmt"
    "net/http"
)

type msg string

func (m msg) ServeHTTP(resp http.ResponseWriter, req *http.Request) {
    fmt.Fprint(resp, m)
}

func main() {
    msgHandler := msg("Hello from Web Server in Go")
    fmt.Println("Server is listening...")
    http.ListenAndServe("localhost:8181", msgHandler)
}
```

**Ключові функції:**
- `http.ListenAndServe(addr, handler)` - запуск веб-сервера
- `Handler` інтерфейс з методом `ServeHTTP(ResponseWriter, *Request)`

### 2. Маршрутизація

#### Функція HandleFunc
```go
func main() {
    http.HandleFunc("/about", func(w http.ResponseWriter, r *http.Request) {
        fmt.Fprint(w, "About Page")
    })

    http.HandleFunc("/contact", func(w http.ResponseWriter, r *http.Request) {
        fmt.Fprint(w, "Contact Page")
    })

    http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
        fmt.Fprint(w, "Index Page")
    })

    fmt.Println("Server is listening...")
    http.ListenAndServe(":8181", nil)
}
```

#### Відправлення файлів
```go
http.HandleFunc("/hello", func(w http.ResponseWriter, r *http.Request) {
    http.ServeFile(w, r, "hello.html")
})
```

#### Функція Handle
```go
type httpHandler struct {
    message string
}

func (h httpHandler) ServeHTTP(resp http.ResponseWriter, req *http.Request) {
    fmt.Fprint(resp, h.message)
}

func main() {
    h1 := httpHandler{message: "Index"}
    h2 := httpHandler{message: "About"}

    http.Handle("/", h1)
    http.Handle("/about", h2)

    http.ListenAndServe(":8181", nil)
}
```

### 3. Статичні файли

#### FileServer
```go
func main() {
    // Відправлення всіх файлів з папки static
    fs := http.FileServer(http.Dir("static"))
    http.Handle("/", fs)

    // Додавання динамічних маршрутів
    http.HandleFunc("/about", func(w http.ResponseWriter, r *http.Request) {
        fmt.Fprint(w, "About Page")
    })

    http.ListenAndServe(":8181", nil)
}
```

#### ServeFile
```go
http.HandleFunc("/about", func(w http.ResponseWriter, r *http.Request) {
    http.ServeFile(w, r, "static/about.html")
})

http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
    http.ServeFile(w, r, "static/index.html")
})
```

### 4. Маршрутизація з gorilla/mux

#### Встановлення
```bash
go get github.com/gorilla/mux
```

#### Використання
```go
import "github.com/gorilla/mux"

func productsHandler(w http.ResponseWriter, r *http.Request) {
    vars := mux.Vars(r)
    id := vars["id"]
    response := fmt.Sprintf("Product %s", id)
    fmt.Fprint(w, response)
}

func main() {
    router := mux.NewRouter()

    // Маршрут з параметром та регулярним виразом
    router.HandleFunc("/products/{id:[0-9]+}", productsHandler)

    // Маршрут з кількома параметрами
    router.HandleFunc("/products/{category}/{id:[0-9]+}",
        func(w http.ResponseWriter, r *http.Request) {
            vars := mux.Vars(r)
            id := vars["id"]
            cat := vars["category"]
            fmt.Fprintf(w, "Product category=%s id=%s", cat, id)
        })

    http.Handle("/", router)
    http.ListenAndServe(":8181", nil)
}
```

### 5. Рядок запиту та форми

#### Отримання параметрів з URL
```go
http.HandleFunc("/user", func(w http.ResponseWriter, r *http.Request) {
    name := r.URL.Query().Get("name")
    age := r.URL.Query().Get("age")
    fmt.Fprintf(w, "Ім'я: %s Вік: %s", name, age)
})
```

Приклад URL: `http://localhost:8181/user?name=Sam&age=21`

#### Отримання даних форми
```html
<!-- user.html -->
<form method="POST" action="postform">
    <label>Ім'я</label><br>
    <input type="text" name="username" /><br><br>
    <label>Вік</label><br>
    <input type="number" name="userage" /><br><br>
    <input type="submit" value="Відправити" />
</form>
```

```go
http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
    http.ServeFile(w, r, "user.html")
})

http.HandleFunc("/postform", func(w http.ResponseWriter, r *http.Request) {
    name := r.FormValue("username")
    age := r.FormValue("userage")
    fmt.Fprintf(w, "Ім'я: %s Вік: %s", name, age)
})
```

### 6. Шаблони (Templates)

#### Простий шаблон
```go
import "html/template"

func main() {
    http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
        data := "Go Template"
        tmpl, _ := template.New("data").Parse("<h1>{{ . }}</h1>")
        tmpl.Execute(w, data)
    })

    http.ListenAndServe(":8181", nil)
}
```

#### Шаблон зі структурою
```go
type ViewData struct {
    Title   string
    Message string
}

func main() {
    http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
        data := ViewData{
            Title:   "World Cup",
            Message: "FIFA will never regret it",
        }

        tmpl := template.Must(template.New("data").Parse(`<div>
            <h1>{{ .Title }}</h1>
            <p>{{ .Message }}</p>
        </div>`))

        tmpl.Execute(w, data)
    })

    http.ListenAndServe(":8181", nil)
}
```

#### Шаблон з файлу
```html
<!-- templates/index.html -->
<!DOCTYPE html>
<html>
    <head>
        <meta charset="UTF-8">
        <title>{{ .Title }}</title>
    </head>
    <body>
        <h1>{{ .Title }}</h1>
        <p>{{ .Message }}</p>
    </body>
</html>
```

```go
func main() {
    http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
        data := ViewData{
            Title:   "World Cup",
            Message: "FIFA will never regret it",
        }

        tmpl, _ := template.ParseFiles("templates/index.html")
        tmpl.Execute(w, data)
    })

    http.ListenAndServe(":8181", nil)
}
```

### 7. Синтаксис шаблонів

#### Цикли
```html
<ul>
    {{range .Users}}
        <li><b>{{ . }}</b></li>
    {{else}}
        <li><b>no rows</b></li>
    {{end}}
</ul>
```

Для масиву структур:
```html
{{range .Users}}
    <li>
        <div><b>{{ .Name }}</b>: {{ .Age }}</div>
    </li>
{{end}}
```

#### Умовні конструкції
```html
<!-- Проста умова -->
{{if .Available}}
    <p>Available</p>
{{else}}
    <p>Not Available</p>
{{end}}

<!-- Порівняння -->
{{if lt .Hour 12}}
    <p>Доброе утро</p>
{{else}}
    <p>Добрий день</p>
{{end}}
```

**Оператори порівняння:**
- `eq` - дорівнює (==)
- `ne` - не дорівнює (!=)
- `lt` - менше (<)
- `le` - менше або дорівнює (<=)
- `gt` - більше (>)
- `ge` - більше або дорівнює (>=)

**Логічні оператори:**
- `and` - логічне І
- `or` - логічне АБО
- `not` - логічне НЕ

```html
{{if or (gt 2 1) (lt 5 7)}}
    <p>Перший варіант</p>
{{else}}
    <p>Другий варіант</p>
{{end}}
```

## Приклади повних додатків

### Простий веб-сайт
```go
package main

import (
    "html/template"
    "net/http"
)

type PageData struct {
    Title   string
    Content string
}

func indexHandler(w http.ResponseWriter, r *http.Request) {
    data := PageData{
        Title:   "Home Page",
        Content: "Welcome to our website!",
    }

    tmpl, _ := template.ParseFiles("templates/index.html")
    tmpl.Execute(w, data)
}

func aboutHandler(w http.ResponseWriter, r *http.Request) {
    data := PageData{
        Title:   "About Us",
        Content: "Information about our company",
    }

    tmpl, _ := template.ParseFiles("templates/about.html")
    tmpl.Execute(w, data)
}

func main() {
    // Статичні файли
    fs := http.FileServer(http.Dir("static"))
    http.Handle("/static/", http.StripPrefix("/static/", fs))

    // Маршрути
    http.HandleFunc("/", indexHandler)
    http.HandleFunc("/about", aboutHandler)

    http.ListenAndServe(":8080", nil)
}
```

### REST API приклад
```go
package main

import (
    "encoding/json"
    "net/http"
    "github.com/gorilla/mux"
)

type Product struct {
    ID    string  `json:"id"`
    Name  string  `json:"name"`
    Price float64 `json:"price"`
}

var products = []Product{
    {ID: "1", Name: "Product 1", Price: 100},
    {ID: "2", Name: "Product 2", Price: 200},
}

func getProducts(w http.ResponseWriter, r *http.Request) {
    w.Header().Set("Content-Type", "application/json")
    json.NewEncoder(w).Encode(products)
}

func getProduct(w http.ResponseWriter, r *http.Request) {
    w.Header().Set("Content-Type", "application/json")
    params := mux.Vars(r)

    for _, item := range products {
        if item.ID == params["id"] {
            json.NewEncoder(w).Encode(item)
            return
        }
    }

    w.WriteHeader(http.StatusNotFound)
    json.NewEncoder(w).Encode(map[string]string{"error": "Product not found"})
}

func main() {
    router := mux.NewRouter()

    router.HandleFunc("/api/products", getProducts).Methods("GET")
    router.HandleFunc("/api/products/{id}", getProduct).Methods("GET")

    http.ListenAndServe(":8080", router)
}
```

## Структура проекту
```
lab4/
├── README.md (цей файл)
├── docs/
│   └── Лабораторна робота 4.docx
├── static/
│   ├── css/
│   ├── js/
│   └── images/
├── templates/
│   ├── index.html
│   └── about.html
└── main.go
```

## Корисні пакети

- `net/http` - базовий HTTP функціонал
- `html/template` - шаблони HTML
- `encoding/json` - робота з JSON
- `github.com/gorilla/mux` - розширена маршрутизація
- `github.com/gorilla/sessions` - сесії
- `github.com/gorilla/websocket` - WebSocket

## Корисні команди
```bash
# Запуск сервера
go run main.go

# Компіляція
go build -o server

# Встановлення залежностей
go get github.com/gorilla/mux

# Запуск з hot reload (потрібен air)
go install github.com/cosmtrek/air@latest
air
```

## Поради

1. **Обробка помилок** - завжди перевіряйте помилки при роботі з шаблонами
2. **Безпека** - використовуйте `html/template` замість `text/template` для запобігання XSS
3. **Структура** - розділяйте логіку на окремі пакети (handlers, models, views)
4. **Middleware** - використовуйте middleware для логування, аутентифікації тощо
5. **Context** - використовуйте `context.Context` для передачі request-scoped значень
