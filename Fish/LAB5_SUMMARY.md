# Лабораторна робота №5 - Звіт

## Виконано

### 1. Додано NuGet пакети

**Fish.Infrastructure.csproj**:
- Microsoft.AspNetCore.Identity.EntityFrameworkCore (v9.0.0)

**Fish.REST.csproj**:
- Microsoft.AspNetCore.Authentication.JwtBearer (v9.0.0)
- Microsoft.EntityFrameworkCore.Design (v9.0.0)
- System.IdentityModel.Tokens.Jwt (v8.2.1)

### 2. Створено модель користувача

**Файл**: `Fish.Infrastructure/Models/ApplicationUser.cs`

Клас `ApplicationUser` наслідується від `IdentityUser` та містить додаткові властивості:
- `FullName` - повне ім'я користувача
- `CreatedAt` - дата створення облікового запису

### 3. Модифіковано DbContext

**Файл**: `Fish.Infrastructure/FishContext.cs`

Змінено базовий клас з `DbContext` на `IdentityDbContext<ApplicationUser>` для підтримки Identity.

### 4. Створено міграцію

**Міграція**: `AddIdentity`

Додано таблиці Identity до бази даних:
- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetUserTokens
- AspNetRoleClaims

### 5. Створено DTO для аутентифікації

**Файли**:
- `Fish.REST/Models/RegisterDto.cs` - для реєстрації
- `Fish.REST/Models/LoginDto.cs` - для входу
- `Fish.REST/Models/AuthResponseDto.cs` - для відповіді з токеном

### 6. Налаштовано JWT

**Файл**: `Fish.REST/appsettings.json`

Додано конфігурацію JWT:
- SecretKey: "FishRestApiSecretKeyForJwtToken2025!"
- Issuer: "FishRestApi"
- Audience: "FishRestApiUsers"
- ExpiryInHours: 24

**Файл**: `Fish.REST/Program.cs`

Налаштовано:
- Identity сервіси з вимогами до паролів
- JWT Authentication з Bearer схемою
- Автоматичне створення ролей при старті (Visitor, Manager, Admin)
- Middleware для аутентифікації

### 7. Створено AuthController

**Файл**: `Fish.REST/Controllers/AuthController.cs`

Реалізовано endpoints:
- `POST /api/auth/register` - реєстрація Visitor
- `POST /api/auth/login` - вхід та отримання JWT токена
- `POST /api/auth/register-manager` - реєстрація Manager (тільки Admin)
- `POST /api/auth/register-admin` - реєстрація Admin (тільки Admin)

### 8. Додано авторизацію до контролерів

**Файли**:
- `Fish.REST/Controllers/FishesController.cs`
- `Fish.REST/Controllers/AquariumsController.cs`

Додано атрибути:
- `[AllowAnonymous]` для GET методів
- `[Authorize(Roles = "Manager,Admin")]` для POST, PUT, DELETE методів

### 9. Налаштовано Swagger UI

**Файл**: `Fish.REST/Program.cs`

Додано підтримку JWT Bearer токенів у Swagger UI:
- SecurityDefinition для Bearer
- SecurityRequirement для автоматичного додавання токена до запитів

## Архітектура

```
┌─────────────┐
│   Client    │
└──────┬──────┘
       │
       │ 1. Register/Login
       ▼
┌─────────────────────┐
│  Auth Endpoints     │
│  /api/auth/*        │
└──────┬──────────────┘
       │
       │ 2. Create User & Generate JWT
       ▼
┌─────────────────────┐
│  Identity Tables    │
│  (PostgreSQL)       │
└─────────────────────┘
       │
       │ 3. Return JWT Token
       ▼
┌─────────────┐
│   Client    │ ─────┐
└─────────────┘      │
                     │ 4. Request with Bearer Token
                     ▼
              ┌─────────────────────┐
              │  API Endpoints      │
              │  /api/fishes/*      │
              │  /api/aquariums/*   │
              └──────┬──────────────┘
                     │
                     │ 5. Validate Token & Check Role
                     ▼
              ┌─────────────────────┐
              │  Authorization      │
              │  Middleware         │
              └──────┬──────────────┘
                     │
                     │ 6. Allow/Deny
                     ▼
              ┌─────────────────────┐
              │  Controller Action  │
              └─────────────────────┘
```

## Ролі та права доступу

| Роль    | GET (Fishes/Aquariums) | POST | PUT | DELETE | Register Manager/Admin |
|---------|------------------------|------|-----|--------|------------------------|
| Visitor | ✅                     | ❌   | ❌  | ❌     | ❌                     |
| Manager | ✅                     | ✅   | ✅  | ✅     | ❌                     |
| Admin   | ✅                     | ✅   | ✅  | ✅     | ✅                     |

## Тестування

Детальна інструкція з тестування знаходиться у файлі `LAB5_TESTING_GUIDE.md`.

Основні сценарії тестування:
1. Реєстрація та вхід користувачів
2. Перегляд даних без токена
3. Спроби модифікації без токена (401)
4. Спроби модифікації з токеном Visitor (403)
5. Успішна модифікація з токеном Manager (200/201)
6. Створення Manager/Admin користувачів

## Використані технології

- ASP.NET Core 9.0
- ASP.NET Core Identity
- JWT Bearer Authentication
- Entity Framework Core 9.0
- PostgreSQL
- Swagger/OpenAPI

## Структура проекту

```
Fish/
├── Fish.Infrastructure/
│   ├── Models/
│   │   └── ApplicationUser.cs          (новий)
│   ├── FishContext.cs                   (модифікований)
│   └── Fish.Infrastructure.csproj       (модифікований)
│
├── Fish.REST/
│   ├── Controllers/
│   │   ├── AuthController.cs           (новий)
│   │   ├── FishesController.cs         (модифікований)
│   │   └── AquariumsController.cs      (модифікований)
│   ├── Models/
│   │   ├── RegisterDto.cs              (новий)
│   │   ├── LoginDto.cs                 (новий)
│   │   └── AuthResponseDto.cs          (новий)
│   ├── Program.cs                       (модифікований)
│   ├── appsettings.json                 (модифікований)
│   └── Fish.REST.csproj                 (модифікований)
│
└── Migrations/
    └── XXXXXX_AddIdentity.cs            (новий)
```

## Висновок

Лабораторна робота №5 успішно виконана. Реалізовано повноцінну систему аутентифікації та авторизації з використанням ASP.NET Core Identity та JWT токенів. Додано три ролі (Visitor, Manager, Admin) з різними рівнями доступу до API endpoints.

Всі контролери захищені відповідними атрибутами авторизації, що забезпечує безпеку REST API. Swagger UI налаштовано для зручного тестування з JWT токенами.

