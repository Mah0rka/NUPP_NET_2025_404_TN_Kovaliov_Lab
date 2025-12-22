# Налаштування Environment Variables

## 📋 Connection String винесено в конфігурацію

Connection string до PostgreSQL тепер зберігається в `appsettings.json` та може бути перевизначений через environment variables.

---

## 🔧 Поточна конфігурація

### Файли конфігурації:

**Fish.REST/appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=FishDb;Username=postgres;Password=123"
  }
}
```

**Fish.MVC/appsettings.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=FishDb;Username=postgres;Password=123"
  }
}
```

---

## 🌍 Використання Environment Variables

### Варіант 1: Через PowerShell (тимчасово для сесії)

```powershell
# Встановити змінну для поточної сесії
$env:ConnectionStrings__DefaultConnection = "Host=localhost;Database=FishDb;Username=myuser;Password=mypassword"

# Запустити проект
cd Fish\Fish.REST
dotnet run
```

**Примітка:** Використовуйте подвійне підкреслення `__` для вкладених секцій JSON.

### Варіант 2: Через launchSettings.json (для розробки)

**Fish.REST/Properties/launchSettings.json:**
```json
{
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": true,
      "applicationUrl": "http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development",
        "ConnectionStrings__DefaultConnection": "Host=localhost;Database=FishDb;Username=postgres;Password=123"
      }
    }
  }
}
```

### Варіант 3: Системні змінні (постійно)

#### Windows:
```powershell
# Встановити для користувача
[System.Environment]::SetEnvironmentVariable("ConnectionStrings__DefaultConnection", "Host=localhost;Database=FishDb;Username=postgres;Password=123", "User")

# Або через GUI:
# 1. Win + R → sysdm.cpl
# 2. Advanced → Environment Variables
# 3. New → 
#    Name: ConnectionStrings__DefaultConnection
#    Value: Host=localhost;Database=FishDb;Username=postgres;Password=123
```

#### Linux/Mac:
```bash
# Додати в ~/.bashrc або ~/.zshrc
export ConnectionStrings__DefaultConnection="Host=localhost;Database=FishDb;Username=postgres;Password=123"

# Перезавантажити
source ~/.bashrc
```

### Варіант 4: Через appsettings.Development.json (рекомендовано)

**Fish.REST/appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=FishDb;Username=postgres;Password=123"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**Переваги:**
- Не треба змінювати системні змінні
- Легко перемикатися між Development/Production
- Файл можна додати в `.gitignore` для безпеки

### Варіант 5: User Secrets (найбезпечніше для розробки)

```powershell
# Ініціалізувати secrets для проекту
cd Fish\Fish.REST
dotnet user-secrets init

# Додати connection string
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=FishDb;Username=postgres;Password=123"

# Переглянути всі secrets
dotnet user-secrets list

# Видалити secret
dotnet user-secrets remove "ConnectionStrings:DefaultConnection"

# Очистити всі secrets
dotnet user-secrets clear
```

**Переваги:**
- Не зберігається в Git
- Зберігається зашифровано на локальній машині
- Автоматично завантажується в Development режимі

---

## 🔐 Безпека

### ❌ НЕ РОБІТЬ:
```csharp
// НЕ хардкодьте паролі в коді!
var connectionString = "Host=localhost;Database=FishDb;Username=postgres;Password=123";
```

### ✅ РОБІТЬ:
```csharp
// Читайте з конфігурації
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string not found.");
```

### Додайте в .gitignore:
```gitignore
# Не комітьте файли з паролями
appsettings.Development.json
appsettings.Production.json
*.user
```

---

## 🎯 Пріоритет завантаження конфігурації

ASP.NET Core завантажує конфігурацію в такому порядку (пізніші перезаписують ранні):

1. `appsettings.json`
2. `appsettings.{Environment}.json` (наприклад, `appsettings.Development.json`)
3. **User Secrets** (тільки в Development)
4. **Environment Variables**
5. **Command-line arguments**

---

## 📝 Приклади для різних середовищ

### Development (локальна розробка):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=FishDb;Username=postgres;Password=123"
  }
}
```

### Production (сервер):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=prod-server;Database=FishDb;Username=prod_user;Password=${DB_PASSWORD}"
  }
}
```

Або через environment variable:
```bash
export ConnectionStrings__DefaultConnection="Host=prod-server;Database=FishDb;Username=prod_user;Password=secure_password"
```

### Docker:
```yaml
# docker-compose.yml
services:
  fish-api:
    image: fish-rest:latest
    environment:
      - ConnectionStrings__DefaultConnection=Host=postgres;Database=FishDb;Username=postgres;Password=postgres
    depends_on:
      - postgres
  
  postgres:
    image: postgres:16
    environment:
      - POSTGRES_DB=FishDb
      - POSTGRES_USER=postgres
      - POSTGRES_PASSWORD=postgres
```

---

## 🧪 Перевірка конфігурації

### Додайте логування connection string (без пароля):

```csharp
var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionStringWithoutPassword = connectionString?.Split(';')
    .Where(s => !s.Contains("Password", StringComparison.OrdinalIgnoreCase))
    .Aggregate((a, b) => $"{a};{b}");

builder.Services.AddDbContext<FishContext>(options =>
{
    options.UseNpgsql(connectionString);
    Console.WriteLine($"Using connection: {connectionStringWithoutPassword}");
});
```

---

## 📚 Корисні команди

```powershell
# Перевірити поточні environment variables
Get-ChildItem Env: | Where-Object { $_.Name -like "*Connection*" }

# Видалити environment variable
Remove-Item Env:ConnectionStrings__DefaultConnection

# Запустити з іншою конфігурацією
dotnet run --environment Production

# Запустити з command-line аргументом
dotnet run --ConnectionStrings:DefaultConnection="Host=other;Database=Test"
```

---

## ✅ Рекомендації

1. **Для розробки:** Використовуйте `appsettings.Development.json` або User Secrets
2. **Для CI/CD:** Використовуйте Environment Variables
3. **Для Production:** Використовуйте Environment Variables або Azure Key Vault / AWS Secrets Manager
4. **Завжди додавайте** `appsettings.Development.json` в `.gitignore`
5. **Ніколи не комітьте** паролі в Git

---

## 🚀 Швидкий старт з User Secrets

```powershell
# Fish.REST
cd C:\Users\user\repos\NUPP_NET_2025_404_TN_Kovaliov_Lab\Fish\Fish.REST
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=FishDb;Username=postgres;Password=123"

# Fish.MVC
cd C:\Users\user\repos\NUPP_NET_2025_404_TN_Kovaliov_Lab\Fish\Fish.MVC
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Database=FishDb;Username=postgres;Password=123"

# Запустити проекти
cd ..\Fish.REST
dotnet run

# В іншому терміналі
cd ..\Fish.MVC
dotnet run
```

Готово! Тепер connection string безпечно винесено з коду! 🔐

