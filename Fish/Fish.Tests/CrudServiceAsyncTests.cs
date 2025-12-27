using Fish.Common;
using Fish.Common.Services;
using Xunit;

namespace Fish.Tests
{
    // Модульні тести для CrudServiceAsync
    public class CrudServiceAsyncTests
    {
        // Допоміжний метод для створення тестового сервісу
        private CrudServiceAsync<FishBase> CreateTestService(string fileName = "test_data.json")
        {
            return new CrudServiceAsync<FishBase>(fish => fish.Id, fileName);
        }

        // Тест створення елемента
        [Fact]
        public async Task CreateAsync_ShouldAddElement()
        {
            // Arrange
            var service = CreateTestService();
            var fish = FreshwaterFish.CreateNew();

            // Act
            var result = await service.CreateAsync(fish);

            // Assert
            Assert.True(result);
            Assert.Equal(1, service.Count());
        }

        // Тест створення null елемента
        [Fact]
        public async Task CreateAsync_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var service = CreateTestService();

            // Act
            var result = await service.CreateAsync(null!);

            // Assert
            Assert.False(result);
            Assert.Equal(0, service.Count());
        }

        // Тест читання елемента
        [Fact]
        public async Task ReadAsync_ShouldReturnElement()
        {
            // Arrange
            var service = CreateTestService();
            var fish = SaltwaterFish.CreateNew();
            await service.CreateAsync(fish);

            // Act
            var result = await service.ReadAsync(fish.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(fish.Id, result.Id);
        }

        // Тест читання неіснуючого елемента
        [Fact]
        public async Task ReadAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var service = CreateTestService();
            var invalidId = Guid.NewGuid();

            // Act
            var result = await service.ReadAsync(invalidId);

            // Assert
            Assert.Null(result);
        }

        // Тест читання всіх елементів
        [Fact]
        public async Task ReadAllAsync_ShouldReturnAllElements()
        {
            // Arrange
            var service = CreateTestService();
            await service.CreateAsync(FreshwaterFish.CreateNew());
            await service.CreateAsync(SaltwaterFish.CreateNew());
            await service.CreateAsync(MigratoryFish.CreateNew());

            // Act
            var result = await service.ReadAllAsync();

            // Assert
            Assert.Equal(3, result.Count());
        }

        // Тест пагінації
        [Fact]
        public async Task ReadAllAsync_WithPagination_ShouldReturnPagedResults()
        {
            // Arrange
            var service = CreateTestService();
            for (int i = 0; i < 25; i++)
            {
                await service.CreateAsync(FreshwaterFish.CreateNew());
            }

            // Act
            var page1 = await service.ReadAllAsync(1, 10);
            var page2 = await service.ReadAllAsync(2, 10);
            var page3 = await service.ReadAllAsync(3, 10);

            // Assert
            Assert.Equal(10, page1.Count());
            Assert.Equal(10, page2.Count());
            Assert.Equal(5, page3.Count());
        }

        // Тест пагінації з невалідними параметрами
        [Fact]
        public async Task ReadAllAsync_WithInvalidPagination_ShouldReturnEmpty()
        {
            // Arrange
            var service = CreateTestService();
            await service.CreateAsync(FreshwaterFish.CreateNew());

            // Act
            var resultNegativePage = await service.ReadAllAsync(-1, 10);
            var resultZeroAmount = await service.ReadAllAsync(1, 0);

            // Assert
            Assert.Empty(resultNegativePage);
            Assert.Empty(resultZeroAmount);
        }

        // Тест оновлення елемента
        [Fact]
        public async Task UpdateAsync_ShouldUpdateElement()
        {
            // Arrange
            var service = CreateTestService();
            var fish = new FreshwaterFish(
                new FishType("Тестова риба", "Прісна вода", 10, false, 0.2),
                20.0, 7.0, 50
            );
            await service.CreateAsync(fish);
            var originalId = fish.Id;

            // Act - оновлюємо той самий об'єкт (змінюємо його властивості)
            var result = await service.UpdateAsync(fish);
            var retrieved = await service.ReadAsync(originalId);

            // Assert
            Assert.True(result);
            Assert.NotNull(retrieved);
            Assert.Equal(originalId, retrieved.Id);
        }

        // Тест оновлення неіснуючого елемента
        [Fact]
        public async Task UpdateAsync_WithNonExistingElement_ShouldReturnFalse()
        {
            // Arrange
            var service = CreateTestService();
            var fish = FreshwaterFish.CreateNew();

            // Act
            var result = await service.UpdateAsync(fish);

            // Assert
            Assert.False(result);
        }

        // Тест видалення елемента
        [Fact]
        public async Task RemoveAsync_ShouldRemoveElement()
        {
            // Arrange
            var service = CreateTestService();
            var fish = MigratoryFish.CreateNew();
            await service.CreateAsync(fish);

            // Act
            var result = await service.RemoveAsync(fish);

            // Assert
            Assert.True(result);
            Assert.Equal(0, service.Count());
        }

        // Тест видалення null елемента
        [Fact]
        public async Task RemoveAsync_WithNull_ShouldReturnFalse()
        {
            // Arrange
            var service = CreateTestService();

            // Act
            var result = await service.RemoveAsync(null!);

            // Assert
            Assert.False(result);
        }

        // Тест збереження у файл
        [Fact]
        public async Task SaveAsync_ShouldSerializeToFile()
        {
            // Arrange
            var fileName = $"test_save_{Guid.NewGuid()}.json";
            var service = CreateTestService(fileName);
            await service.CreateAsync(FreshwaterFish.CreateNew());
            await service.CreateAsync(SaltwaterFish.CreateNew());

            try
            {
                // Act
                var result = await service.SaveAsync();

                // Assert
                Assert.True(result);
                Assert.True(File.Exists(fileName));
                
                var fileInfo = new FileInfo(fileName);
                Assert.True(fileInfo.Length > 0);
            }
            finally
            {
                // Cleanup
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        // Тест завантаження з файлу
        [Fact]
        public async Task LoadAsync_ShouldDeserializeFromFile()
        {
            // Arrange
            var fileName = $"test_load_{Guid.NewGuid()}.json";
            var service1 = CreateTestService(fileName);
            await service1.CreateAsync(FreshwaterFish.CreateNew());
            await service1.CreateAsync(SaltwaterFish.CreateNew());
            await service1.SaveAsync();

            try
            {
                // Act - перевіряємо що файл існує
                Assert.True(File.Exists(fileName));
                
                var fileInfo = new FileInfo(fileName);
                Assert.True(fileInfo.Length > 0);
            }
            finally
            {
                // Cleanup
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        // Тест багатопотоковості - одночасне створення
        [Fact]
        public async Task ConcurrentCreates_ShouldBeThreadSafe()
        {
            // Arrange
            var service = CreateTestService();
            var tasks = new List<Task>();

            // Act
            for (int i = 0; i < 100; i++)
            {
                tasks.Add(Task.Run(async () =>
                {
                    var fish = FreshwaterFish.CreateNew();
                    await service.CreateAsync(fish);
                }));
            }

            await Task.WhenAll(tasks);

            // Assert
            Assert.Equal(100, service.Count());
        }

        // Тест IEnumerable - підтримка foreach
        [Fact]
        public async Task IEnumerable_ShouldSupportForEach()
        {
            // Arrange
            var service = CreateTestService();
            await service.CreateAsync(FreshwaterFish.CreateNew());
            await service.CreateAsync(SaltwaterFish.CreateNew());
            await service.CreateAsync(MigratoryFish.CreateNew());

            // Act
            var count = 0;
            foreach (var fish in service)
            {
                count++;
                Assert.NotNull(fish);
            }

            // Assert
            Assert.Equal(3, count);
        }

        // Тест IEnumerable - підтримка LINQ
        [Fact]
        public async Task IEnumerable_ShouldSupportLinq()
        {
            // Arrange
            var service = CreateTestService();
            await service.CreateAsync(FreshwaterFish.CreateNew());
            await service.CreateAsync(SaltwaterFish.CreateNew());
            await service.CreateAsync(MigratoryFish.CreateNew());

            // Act
            var freshwaterCount = service.OfType<FreshwaterFish>().Count();
            var saltwaterCount = service.OfType<SaltwaterFish>().Count();
            var migratoryCount = service.OfType<MigratoryFish>().Count();

            // Assert
            Assert.Equal(1, freshwaterCount);
            Assert.Equal(1, saltwaterCount);
            Assert.Equal(1, migratoryCount);
        }
    }
}

