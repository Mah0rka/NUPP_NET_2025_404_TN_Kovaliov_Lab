# Лабораторна робота №5: ASP.NET Core Identity та JWT Authentication

## Огляд виконаної роботи

Реалізовано повноцінну систему аутентифікації та авторизації для Fish REST API з використанням ASP.NET Core Identity та JWT токенів.

## Що було зроблено

### ✅ 1. Додано NuGet пакети
- `Microsoft.AspNetCore.Identity.EntityFrameworkCore` до Fish.Infrastructure
- `Microsoft.AspNetCore.Authentication.JwtBearer` до Fish.REST
- `System.IdentityModel.Tokens.Jwt` до Fish.REST
- `Microsoft.EntityFrameworkCore.Design` до Fish.REST

### ✅ 2. Створено модель користувача
- `ApplicationUser` наслідується від `IdentityUser`
- Додано властивості: `FullName`, `CreatedAt`

### ✅ 3. Модифіковано DbContext
- `FishContext` тепер наслідується від `IdentityDbContext<ApplicationUser>`
- Підтримка таблиць Identity в PostgreSQL

### ✅ 4. Створено міграцію AddIdentity
- Додано всі необхідні таблиці Identity до бази даних

### ✅ 5. Створено DTO для аутентифікації
- `RegisterDto` - реєстрація з валідацією
- `LoginDto` - вхід
- `AuthResponseDto` - відповідь з JWT токеном

### ✅ 6. Налаштовано JWT
- Конфігурація в `appsettings.json`
- JWT Authentication middleware
- Час життя токена: 24 години

### ✅ 7. Створено AuthController
- `POST /api/auth/register` - реєстрація Visitor
- `POST /api/auth/login` - вхід та генерація токена
- `POST /api/auth/register-manager` - реєстрація Manager (тільки Admin)
- `POST /api/auth/register-admin` - реєстрація Admin (тільки Admin)

### ✅ 8. Додано авторизацію до контролерів
- GET методи доступні всім (`[AllowAnonymous]`)
- POST/PUT/DELETE доступні тільки Manager та Admin

### ✅ 9. Налаштовано Swagger UI
- Підтримка JWT Bearer токенів
- Кнопка "Authorize" для введення токена

## Ролі та права доступу

| Операція | Visitor | Manager | Admin |
|----------|---------|---------|-------|
| GET /api/fishes | ✅ | ✅ | ✅ |
| GET /api/aquariums | ✅ | ✅ | ✅ |
| POST /api/fishes | ❌ | ✅ | ✅ |
| PUT /api/fishes/{id} | ❌ | ✅ | ✅ |
| DELETE /api/fishes/{id} | ❌ | ✅ | ✅ |
| POST /api/aquariums | ❌ | ✅ | ✅ |
| PUT /api/aquariums/{id} | ❌ | ✅ | ✅ |
| DELETE /api/aquariums/{id} | ❌ | ✅ | ✅ |
| POST /api/auth/register-manager | ❌ | ❌ | ✅ |
| POST /api/auth/register-admin | ❌ | ❌ | ✅ |

## Запуск проекту

```bash
cd Fish/Fish.REST
dotnet run
```

Swagger UI: `https://localhost:5001/swagger`

## Швидкий старт тестування

### 1. Реєстрація користувача
```bash
POST https://localhost:5001/api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!",
  "confirmPassword": "Test123!",
  "fullName": "Test User"
}
```

### 2. Вхід
```bash
POST https://localhost:5001/api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!"
}
```

Відповідь містить JWT токен:
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "test@example.com",
  "roles": ["Visitor"]
}
```

### 3. Використання токена

У Swagger UI:
1. Натисніть "Authorize"
2. Введіть: `Bearer {ваш_токен}`
3. Натисніть "Authorize"

У Postman/curl:
```bash
GET https://localhost:5001/api/fishes
Authorization: Bearer {ваш_токен}
```

## Створення першого Admin

Оскільки для створення Admin потрібен токен Admin, перший Admin створюється через SQL:

```sql
-- 1. Створити користувача через /api/auth/register
-- 2. Знайти його ID
SELECT "Id", "Email" FROM "AspNetUsers" WHERE "Email" = 'admin@example.com';

-- 3. Знайти ID ролі Admin
SELECT "Id", "Name" FROM "AspNetRoles" WHERE "Name" = 'Admin';

-- 4. Додати роль Admin користувачу
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId") 
VALUES ('user_id_тут', 'admin_role_id_тут');
```

Після цього увійдіть знову через `/api/auth/login` щоб отримати токен з роллю Admin.

## Структура файлів

### Нові файли:
```
Fish/
├── Fish.Infrastructure/
│   └── Models/
│       └── ApplicationUser.cs
│
├── Fish.REST/
│   ├── Controllers/
│   │   └── AuthController.cs
│   └── Models/
│       ├── RegisterDto.cs
│       ├── LoginDto.cs
│       └── AuthResponseDto.cs
│
└── LAB5_TESTING_GUIDE.md
└── LAB5_SUMMARY.md
└── LAB5_README.md (цей файл)
```

### Модифіковані файли:
```
Fish/
├── Fish.Infrastructure/
│   ├── FishContext.cs
│   └── Fish.Infrastructure.csproj
│
└── Fish.REST/
    ├── Controllers/
    │   ├── FishesController.cs
    │   └── AquariumsController.cs
    ├── Program.cs
    ├── appsettings.json
    └── Fish.REST.csproj
```

## Тестування

Детальна інструкція з тестування знаходиться у файлі **LAB5_TESTING_GUIDE.md**.

Для створення PDF звіту виконайте всі тестові сценарії з цього файлу та зробіть скріншоти результатів.

## Технічні деталі

### JWT Configuration
- **Algorithm**: HS256 (HMAC-SHA256)
- **Secret Key**: 40 символів
- **Issuer**: FishRestApi
- **Audience**: FishRestApiUsers
- **Expiry**: 24 години

### Password Requirements
- Мінімум 6 символів
- Мінімум 1 цифра
- Мінімум 1 велика літера
- Мінімум 1 мала літера
- Спеціальні символи не обов'язкові

### Identity Tables
- AspNetUsers - користувачі
- AspNetRoles - ролі
- AspNetUserRoles - зв'язок користувач-роль
- AspNetUserClaims - додаткові claims користувачів
- AspNetUserLogins - зовнішні логіни
- AspNetUserTokens - токени для скидання пароля тощо
- AspNetRoleClaims - claims ролей

## Безпека

✅ Паролі хешуються за допомогою Identity (PBKDF2)
✅ JWT токени підписуються секретним ключем
✅ HTTPS для production
✅ Валідація токенів на кожному запиті
✅ Role-based authorization
✅ Email повинен бути унікальним

## Можливі помилки та рішення

### 401 Unauthorized
- Токен не надано
- Токен невалідний або прострочений
- **Рішення**: Отримайте новий токен через `/api/auth/login`

### 403 Forbidden
- Токен валідний, але недостатньо прав
- **Рішення**: Використайте токен з відповідною роллю

### 400 Bad Request
- Невалідні дані у запиті
- Паролі не співпадають
- Email вже використовується
- **Рішення**: Перевірте дані запиту

## Контрольні запитання

1. **Які ключові особливості ASP.NET Core Identity?**
   - Управління користувачами та ролями
   - Хешування паролів
   - Валідація даних
   - Підтримка зовнішніх провайдерів

2. **Як працює JWT аутентифікація?**
   - Клієнт надсилає credentials
   - Сервер генерує підписаний JWT токен
   - Клієнт зберігає токен
   - Токен надсилається з кожним запитом у заголовку Authorization

3. **Чому GET методи доступні без авторизації?**
   - Перегляд публічної інформації не потребує аутентифікації
   - Модифікація даних потребує авторизації

4. **Як створити першого Admin користувача?**
   - Через SQL додати роль до існуючого користувача
   - Або тимчасово видалити атрибут `[Authorize]` з методу RegisterAdmin

## Висновок

Лабораторна робота №5 успішно виконана. Реалізовано:
- ✅ ASP.NET Core Identity
- ✅ JWT Bearer Authentication
- ✅ Role-based Authorization
- ✅ 3 ролі (Visitor, Manager, Admin)
- ✅ Захищені API endpoints
- ✅ Swagger UI з підтримкою JWT

Система готова до тестування та створення PDF звіту.

