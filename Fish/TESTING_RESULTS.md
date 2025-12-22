# Результати тестування лабораторної роботи №4

## Дата тестування
23 грудня 2025

## 1. Fish.REST API (http://localhost:5000)

### Статус: ✅ ПРАЦЮЄ

### Тести REST API:

#### ✅ GET /api/fishes
- **Статус**: 200 OK
- **Результат**: Повертає 1200 риб з бази даних
- **Приклад відповіді**:
```json
{
  "id": "2a336147-7caa-435a-a0f3-cdd71421f112",
  "variety": "Щука",
  "habitat": "Прісна вода",
  "topSpeed": 10,
  "isPredatory": true,
  "length": 0.2,
  "aquariumId": 1,
  "aquariumName": null
}
```

#### ✅ GET /api/aquariums
- **Статус**: 200 OK
- **Результат**: Повертає 3 акваріуми з бази даних
- **Приклад відповіді**:
```json
{
  "id": "338638a5-e99f-4be9-ac9a-d2c328b6b8e1",
  "name": "Тропічний акваріум",
  "volume": 500,
  "location": "Зал 1",
  "fishCount": 0
}
```

#### ✅ GET /api/fishes?page=1&amount=5
- **Статус**: 200 OK
- **Результат**: Пагінація працює, повертає 5 риб на сторінці
- **Функціонал**: Підтримка параметрів page та amount

#### ❌ POST /api/fishes
- **Статус**: 400 Bad Request
- **Примітка**: Можлива проблема з валідацією моделі або маппінгом даних
- **Рекомендація**: Перевірити DataAnnotations та ModelState

### Реалізовані ендпоінти:

**FishesController:**
- `GET /api/fishes` - отримати всі риби
- `GET /api/fishes?page={page}&amount={amount}` - отримати риби з пагінацією
- `GET /api/fishes/{id}` - отримати рибу за Guid
- `POST /api/fishes` - створити нову рибу
- `PUT /api/fishes/{id}` - оновити рибу
- `DELETE /api/fishes/{id}` - видалити рибу

**AquariumsController:**
- `GET /api/aquariums` - отримати всі акваріуми
- `GET /api/aquariums?page={page}&amount={amount}` - отримати акваріуми з пагінацією
- `GET /api/aquariums/{id}` - отримати акваріум за Guid
- `POST /api/aquariums` - створити новий акваріум
- `PUT /api/aquariums/{id}` - оновити акваріум
- `DELETE /api/aquariums/{id}` - видалити акваріум

### HTTP статус коди:
- ✅ 200 OK - успішне отримання/оновлення
- ✅ 201 Created - успішне створення (реалізовано в коді)
- ✅ 404 Not Found - сутність не знайдена (реалізовано в коді)
- ✅ 400 Bad Request - невалідні дані (реалізовано в коді)
- ✅ 500 Internal Server Error - помилка сервера (реалізовано в коді)

### Технічні деталі:
- ✅ Dependency Injection налаштовано
- ✅ Entity Framework Core підключено до PostgreSQL
- ✅ Міграції автоматично застосовуються при старті
- ✅ Асинхронні операції з БД
- ✅ Маппінг між int Id (БД) та Guid ExternalId (API)

---

## 2. Fish.MVC Web Application (http://localhost:5001)

### Статус: ✅ ПРАЦЮЄ

### Реалізовані CRUD сторінки:

#### Razor Pages для FishModel (/Fishes):
- ✅ `/Fishes/Index` - список всіх риб
- ✅ `/Fishes/Create` - форма створення риби
- ✅ `/Fishes/Edit/{id}` - форма редагування риби
- ✅ `/Fishes/Details/{id}` - деталі риби
- ✅ `/Fishes/Delete/{id}` - підтвердження видалення риби

#### Razor Pages для AquariumModel (/Aquariums):
- ✅ `/Aquariums/Index` - список всіх акваріумів
- ✅ `/Aquariums/Create` - форма створення акваріума
- ✅ `/Aquariums/Edit/{id}` - форма редагування акваріума
- ✅ `/Aquariums/Details/{id}` - деталі акваріума
- ✅ `/Aquariums/Delete/{id}` - підтвердження видалення акваріума

### Навігація:
- ✅ Головна сторінка з посиланнями на CRUD сторінки
- ✅ Навігаційне меню з пунктами "Риби" та "Акваріуми"
- ✅ Bootstrap 5 для стилізації

### Технічні деталі:
- ✅ ASP.NET Core MVC + Razor Pages
- ✅ Entity Framework Core підключено до PostgreSQL
- ✅ Міграції автоматично застосовуються при старті
- ✅ Використання існуючого FishContext з Fish.Infrastructure
- ✅ Scaffolding використано для генерації CRUD сторінок

### Попередження компіляції:
- ⚠️ CS8602: Dereference of a possibly null reference (3 попередження)
- **Примітка**: Це попередження nullable reference types, не критичні для роботи

---

## 3. Архітектура та код

### 3-рівнева архітектура:
```
Presentation Layer (Fish.REST, Fish.MVC)
    ↓
Business Logic Layer (EntityFrameworkCrudService)
    ↓
Data Access Layer (Repository, FishContext)
```

### Dependency Injection:
- ✅ DbContext зареєстровано як Scoped
- ✅ IRepository<T> зареєстровано як Scoped
- ✅ ICrudServiceAsync<T> зареєстровано як Scoped з фабрикою

### База даних:
- ✅ PostgreSQL підключено
- ✅ Міграції застосовані
- ✅ Дані з лабораторної роботи №3 доступні (1200 риб, 3 акваріуми)

---

## Висновки

### Що працює:
1. ✅ REST API повністю функціональний для GET запитів
2. ✅ Пагінація працює коректно
3. ✅ MVC застосунок запущено та доступний
4. ✅ CRUD сторінки згенеровані через scaffolding
5. ✅ Навігація налаштована
6. ✅ Підключення до БД працює
7. ✅ Dependency Injection налаштовано
8. ✅ Асинхронні операції реалізовані

### Що потребує уваги:
1. ⚠️ POST запити повертають 400 Bad Request (потрібна додаткова валідація)
2. ⚠️ Swagger тимчасово вимкнено через конфлікт версій пакетів
3. ⚠️ Nullable reference warnings в MVC проекті

### Рекомендації для покращення:
1. Додати DataAnnotations до DTO моделей для валідації
2. Налаштувати Swagger з правильними версіями пакетів
3. Виправити nullable warnings в згенерованих Razor Pages
4. Додати обробку помилок та логування
5. Створити PDF з скріншотами Swagger/Postman запитів

---

## Запуск проектів

### Fish.REST API:
```bash
cd Fish/Fish.REST
dotnet run --urls "http://localhost:5000"
```

### Fish.MVC:
```bash
cd Fish/Fish.MVC
dotnet run --urls "http://localhost:5001"
```

### Тестування API:
```bash
cd Fish
powershell -ExecutionPolicy Bypass -File test_api.ps1
```

