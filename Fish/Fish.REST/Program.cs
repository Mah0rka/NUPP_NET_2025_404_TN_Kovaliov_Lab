using Fish.Common.Services;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;
using Fish.REST.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Налаштування підключення до PostgreSQL
var connectionString = "Host=localhost;Database=FishDb;Username=postgres;Password=123";
builder.Services.AddDbContext<FishContext>(options =>
    options.UseNpgsql(connectionString));

// Реєстрація репозиторіїв
builder.Services.AddScoped<IRepository<FishModel>, Repository<FishModel>>();
builder.Services.AddScoped<IRepository<AquariumModel>, Repository<AquariumModel>>();

// Реєстрація CRUD сервісів
builder.Services.AddScoped<ICrudServiceAsync<FishModel>>(provider =>
{
    var repository = provider.GetRequiredService<IRepository<FishModel>>();
    return new EntityFrameworkCrudService<FishModel>(
        repository,
        fish => fish.ExternalId,
        fish => fish.Id,
        (fish, guid) => fish.ExternalId = guid
    );
});

builder.Services.AddScoped<ICrudServiceAsync<AquariumModel>>(provider =>
{
    var repository = provider.GetRequiredService<IRepository<AquariumModel>>();
    return new EntityFrameworkCrudService<AquariumModel>(
        repository,
        aquarium => aquarium.ExternalId,
        aquarium => aquarium.Id,
        (aquarium, guid) => aquarium.ExternalId = guid
    );
});

// Додавання контролерів
builder.Services.AddControllers();

// Налаштування Swagger/OpenAPI (тимчасово вимкнено через конфлікт версій)
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Застосування міграцій при старті
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FishContext>();
    try
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("Міграції успішно застосовані");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка застосування міграцій: {ex.Message}");
    }
}

// Налаштування HTTP pipeline
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
