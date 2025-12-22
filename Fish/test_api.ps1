# Тестування REST API
$baseUrl = "http://localhost:5000"

Write-Host "=== Тестування Fish REST API ===" -ForegroundColor Green

# Тест 1: GET /api/fishes
Write-Host "`n1. GET /api/fishes - отримати всі риби" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes" -Method GET
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Кількість риб: $($response.Count)"
    if ($response.Count -gt 0) {
        Write-Host "Перша риба: $($response[0] | ConvertTo-Json -Depth 2)"
    }
} catch {
    Write-Host "Помилка: $_" -ForegroundColor Red
}

# Тест 2: GET /api/aquariums
Write-Host "`n2. GET /api/aquariums - отримати всі акваріуми" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/aquariums" -Method GET
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Кількість акваріумів: $($response.Count)"
    if ($response.Count -gt 0) {
        Write-Host "Перший акваріум: $($response[0] | ConvertTo-Json -Depth 2)"
    }
} catch {
    Write-Host "Помилка: $_" -ForegroundColor Red
}

# Тест 3: POST /api/fishes - створити нову рибу
Write-Host "`n3. POST /api/fishes - створити нову рибу" -ForegroundColor Yellow
try {
    $newFish = @{
        variety = "Тестова риба"
        habitat = "Тестове середовище"
        topSpeed = 25
        isPredatory = $false
        length = 15.5
        aquariumId = $null
    }
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes" -Method POST -Body ($newFish | ConvertTo-Json) -ContentType "application/json"
    Write-Host "Статус: Created (201)" -ForegroundColor Green
    Write-Host "Створена риба: $($response | ConvertTo-Json -Depth 2)"
    $createdFishId = $response.id
    
    # Тест 4: GET /api/fishes/{id} - отримати створену рибу
    Write-Host "`n4. GET /api/fishes/$createdFishId - отримати створену рибу" -ForegroundColor Yellow
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes/$createdFishId" -Method GET
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Риба: $($response | ConvertTo-Json -Depth 2)"
    
    # Тест 5: PUT /api/fishes/{id} - оновити рибу
    Write-Host "`n5. PUT /api/fishes/$createdFishId - оновити рибу" -ForegroundColor Yellow
    $newFish.variety = "Оновлена тестова риба"
    $newFish.topSpeed = 30
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes/$createdFishId" -Method PUT -Body ($newFish | ConvertTo-Json) -ContentType "application/json"
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Відповідь: $response"
    
    # Тест 6: DELETE /api/fishes/{id} - видалити рибу
    Write-Host "`n6. DELETE /api/fishes/$createdFishId - видалити рибу" -ForegroundColor Yellow
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes/$createdFishId" -Method DELETE
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Відповідь: $response"
    
} catch {
    Write-Host "Помилка: $_" -ForegroundColor Red
}

# Тест 7: GET /api/fishes?page=1&amount=5 - пагінація
Write-Host "`n7. GET /api/fishes?page=1&amount=5 - пагінація" -ForegroundColor Yellow
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/fishes?page=1&amount=5" -Method GET
    Write-Host "Статус: OK" -ForegroundColor Green
    Write-Host "Кількість риб на сторінці: $($response.Count)"
} catch {
    Write-Host "Помилка: $_" -ForegroundColor Red
}

Write-Host "`n=== Тестування завершено ===" -ForegroundColor Green

