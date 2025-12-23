# Лабораторна робота №5 - Інструкція з тестування

## Огляд

Цей документ містить інструкції для тестування REST API з ASP.NET Core Identity, JWT аутентифікацією та авторизацією на основі ролей.

## Ролі та права доступу

- **Visitor**: Перегляд риб та акваріумів (GET запити)
- **Manager**: Всі операції з рибами та акваріумами (GET, POST, PUT, DELETE)
- **Admin**: Всі операції + можливість створювати Manager та Admin користувачів

## Запуск додатку

```bash
cd Fish/Fish.REST
dotnet run
```

Додаток буде доступний за адресою: `https://localhost:5001` або `http://localhost:5000`

Swagger UI: `https://localhost:5001/swagger`

## Тестові сценарії

### 1. Реєстрація нового користувача (Visitor)

**Endpoint**: `POST /api/auth/register`

**Request Body**:
```json
{
  "email": "visitor@test.com",
  "password": "Visitor123!",
  "confirmPassword": "Visitor123!",
  "fullName": "Test Visitor"
}
```

**Очікуваний результат**: 
- Статус: 200 OK
- Повертає JWT токен та роль "Visitor"

---

### 2. Вхід користувача

**Endpoint**: `POST /api/auth/login`

**Request Body**:
```json
{
  "email": "visitor@test.com",
  "password": "Visitor123!"
}
```

**Очікуваний результат**: 
- Статус: 200 OK
- Повертає JWT токен та список ролей

---

### 3. Перегляд риб без токена (дозволено)

**Endpoint**: `GET /api/fishes`

**Headers**: Без Authorization

**Очікуваний результат**: 
- Статус: 200 OK
- Повертає список риб

---

### 4. Перегляд акваріумів без токена (дозволено)

**Endpoint**: `GET /api/aquariums`

**Headers**: Без Authorization

**Очікуваний результат**: 
- Статус: 200 OK
- Повертає список акваріумів

---

### 5. Створення риби без токена (заборонено)

**Endpoint**: `POST /api/fishes`

**Headers**: Без Authorization

**Request Body**:
```json
{
  "variety": "Золота рибка",
  "habitat": "Прісна вода",
  "topSpeed": 5.5,
  "isPredatory": false,
  "length": 15.0
}
```

**Очікуваний результат**: 
- Статус: 401 Unauthorized

---

### 6. Створення риби з токеном Visitor (заборонено)

**Endpoint**: `POST /api/fishes`

**Headers**: 
```
Authorization: Bearer {visitor_token}
```

**Request Body**:
```json
{
  "variety": "Золота рибка",
  "habitat": "Прісна вода",
  "topSpeed": 5.5,
  "isPredatory": false,
  "length": 15.0
}
```

**Очікуваний результат**: 
- Статус: 403 Forbidden

---

### 7. Створення першого Admin користувача (вручну)

Оскільки для створення Admin потрібен токен Admin, перший Admin створюється вручну через базу даних або додається seed даних.

**Альтернатива**: Тимчасово видалити атрибут `[Authorize(Roles = "Admin")]` з методу `RegisterAdmin`, створити Admin, потім повернути атрибут.

Або використати SQL:

```sql
-- Знайти UserId та RoleId
SELECT * FROM "AspNetUsers" WHERE "Email" = 'visitor@test.com';
SELECT * FROM "AspNetRoles" WHERE "Name" = 'Admin';

-- Додати роль Admin користувачу
INSERT INTO "AspNetUserRoles" ("UserId", "RoleId") 
VALUES ('user_id_here', 'admin_role_id_here');
```

---

### 8. Створення Manager користувача з токеном Admin

**Endpoint**: `POST /api/auth/register-manager`

**Headers**: 
```
Authorization: Bearer {admin_token}
```

**Request Body**:
```json
{
  "email": "manager@test.com",
  "password": "Manager123!",
  "confirmPassword": "Manager123!",
  "fullName": "Test Manager"
}
```

**Очікуваний результат**: 
- Статус: 200 OK
- Повертає JWT токен з роллю "Manager"

---

### 9. Створення Manager користувача з токеном Visitor (заборонено)

**Endpoint**: `POST /api/auth/register-manager`

**Headers**: 
```
Authorization: Bearer {visitor_token}
```

**Request Body**:
```json
{
  "email": "manager2@test.com",
  "password": "Manager123!",
  "confirmPassword": "Manager123!",
  "fullName": "Test Manager 2"
}
```

**Очікуваний результат**: 
- Статус: 403 Forbidden

---

### 10. Створення риби з токеном Manager (дозволено)

**Endpoint**: `POST /api/fishes`

**Headers**: 
```
Authorization: Bearer {manager_token}
```

**Request Body**:
```json
{
  "variety": "Окунь",
  "habitat": "Прісна вода",
  "topSpeed": 12.5,
  "isPredatory": true,
  "length": 25.0
}
```

**Очікуваний результат**: 
- Статус: 201 Created
- Повертає створену рибу з ID

---

### 11. Оновлення риби з токеном Manager (дозволено)

**Endpoint**: `PUT /api/fishes/{id}`

**Headers**: 
```
Authorization: Bearer {manager_token}
```

**Request Body**:
```json
{
  "variety": "Окунь великий",
  "habitat": "Прісна вода",
  "topSpeed": 15.0,
  "isPredatory": true,
  "length": 30.0
}
```

**Очікуваний результат**: 
- Статус: 200 OK

---

### 12. Видалення риби з токеном Manager (дозволено)

**Endpoint**: `DELETE /api/fishes/{id}`

**Headers**: 
```
Authorization: Bearer {manager_token}
```

**Очікуваний результат**: 
- Статус: 200 OK

---

### 13. Створення акваріума з токеном Admin (дозволено)

**Endpoint**: `POST /api/aquariums`

**Headers**: 
```
Authorization: Bearer {admin_token}
```

**Request Body**:
```json
{
  "name": "Великий океанський акваріум",
  "volume": 5000.0,
  "location": "Головний зал"
}
```

**Очікуваний результат**: 
- Статус: 201 Created
- Повертає створений акваріум з ID

---

## Використання Swagger UI

1. Відкрийте `https://localhost:5001/swagger`
2. Виконайте реєстрацію через `/api/auth/register`
3. Скопіюйте отриманий токен
4. Натисніть кнопку "Authorize" у правому верхньому куті
5. Введіть: `Bearer {ваш_токен}`
6. Натисніть "Authorize"
7. Тепер всі запити будуть виконуватися з цим токеном

## Використання Postman

1. Створіть новий Request
2. Виберіть метод (GET, POST, PUT, DELETE)
3. Введіть URL endpoint
4. У вкладці "Headers" додайте:
   - Key: `Authorization`
   - Value: `Bearer {ваш_токен}`
5. У вкладці "Body" виберіть "raw" та "JSON" для POST/PUT запитів
6. Введіть JSON тіло запиту
7. Натисніть "Send"

## Створення PDF звіту

Для PDF звіту включіть скріншоти наступних тестів:

1. ✅ Успішна реєстрація Visitor
2. ✅ Успішний вхід
3. ✅ GET запит без токена (успішно)
4. ❌ POST запит без токена (401)
5. ❌ POST запит з токеном Visitor (403)
6. ✅ POST запит з токеном Manager (201)
7. ✅ PUT запит з токеном Manager (200)
8. ✅ DELETE запит з токеном Manager (200)
9. ❌ POST /register-manager з токеном Visitor (403)
10. ✅ POST /register-manager з токеном Admin (200)

## Перевірка бази даних

Після виконання тестів можна перевірити створені таблиці Identity:

```sql
-- Перегляд користувачів
SELECT * FROM "AspNetUsers";

-- Перегляд ролей
SELECT * FROM "AspNetRoles";

-- Перегляд зв'язків користувач-роль
SELECT u."Email", r."Name" as "Role"
FROM "AspNetUsers" u
JOIN "AspNetUserRoles" ur ON u."Id" = ur."UserId"
JOIN "AspNetRoles" r ON ur."RoleId" = r."Id";
```

## Примітки

- JWT токени дійсні протягом 24 годин
- Паролі повинні містити мінімум 6 символів, включаючи цифри, великі та малі літери
- Email повинен бути унікальним
- Ролі створюються автоматично при запуску додатку

