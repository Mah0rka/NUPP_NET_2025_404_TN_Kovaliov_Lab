# Лабораторна робота №4 - Звіт

## Виконані завдання

### 1. Створено гілку lab4 від lab3
✅ Гілка `lab4` успішно створена від гілки `lab3`

### 2. Проект Fish.REST (ASP.NET Core Web API)

#### Створені файли:
- **Models/** - DTO моделі для API
  - `FishDto.cs` - для створення/оновлення риб
  - `FishResponseDto.cs` - для відповідей з даними риб
  - `AquariumDto.cs` - для створення/оновлення акваріумів
  - `AquariumResponseDto.cs` - для відповідей з даними акваріумів

- **Services/**
  - `EntityFrameworkCrudService.cs` - реалізація ICrudServiceAsync<T> з використанням IRepository<T>

- **Controllers/**
  - `FishesController.cs` - REST API для риб
  - `AquariumsController.cs` - REST API для акваріумів

#### Реалізовані HTTP методи:
- **GET** `/api/fishes` - отримати всі риби (з підтримкою пагінації)
- **GET** `/api/fishes/{id}` - отримати рибу за Guid
- **POST** `/api/fishes` - створити нову рибу (повертає 201 Created)
- **PUT** `/api/fishes/{id}` - оновити рибу
- **DELETE** `/api/fishes/{id}` - видалити рибу

Аналогічні методи для `/api/aquariums`

#### HTTP статус коди:
- 200 OK - успішне отримання/оновлення
- 201 Created - успішне створення
- 404 Not Found - сутність не знайдена
- 400 Bad Request - невалідні дані
- 500 Internal Server Error - помилка сервера

#### Dependency Injection:
У `Program.cs` налаштовано:
- DbContext з PostgreSQL
- IRepository<T> для FishModel та AquariumModel
- ICrudServiceAsync<T> з EntityFrameworkCrudService
- Swagger для документації API

### 3. Проект Fish.MVC (ASP.NET Core MVC + Razor Pages)

#### Створені CRUD сторінки через scaffolding:
- **Pages/Fishes/** - повний CRUD для FishModel
  - Index.cshtml - список риб
  - Create.cshtml - створення риби
  - Edit.cshtml - редагування риби
  - Details.cshtml - деталі риби
  - Delete.cshtml - видалення риби

- **Pages/Aquariums/** - повний CRUD для AquariumModel
  - Index.cshtml - список акваріумів
  - Create.cshtml - створення акваріума
  - Edit.cshtml - редагування акваріума
  - Details.cshtml - деталі акваріума
  - Delete.cshtml - видалення акваріума

#### Налаштована навігація:
- Оновлено головну сторінку (`Views/Home/Index.cshtml`) з посиланнями на CRUD сторінки
- Додано пункти меню в `_Layout.cshtml` для швидкого доступу до риб та акваріумів

#### Налаштування:
- DbContext з PostgreSQL
- Автоматичне застосування міграцій при старті
- Підтримка Razor Pages та MVC контролерів

### 4. Оновлено Fish.sln
✅ Додано проекти Fish.REST та Fish.MVC до solution

## Технічні деталі

### Архітектура:
- **3-рівнева архітектура**: Presentation (REST/MVC) → Business Logic (Services) → Data Access (Repository/EF)
- **Dependency Injection**: використано вбудований DI контейнер ASP.NET Core
- **Маппінг**: між `int Id` (БД) та `Guid ExternalId` (API)

### Використані технології:
- ASP.NET Core 9.0
- Entity Framework Core 9.0
- PostgreSQL (Npgsql)
- Swagger/OpenAPI
- Razor Pages
- Bootstrap 5

### Стиль коду:
- Простий код без ускладнень
- Коментарі українською мовою
- Async/await для всіх операцій з БД

## Компіляція
✅ Весь solution компілюється без помилок та попереджень

## Commit
✅ Зміни збережено в commit: "Lab 4: Added Fish.REST Web API and Fish.MVC projects with CRUD functionality"

