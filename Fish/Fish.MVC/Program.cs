using Fish.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Налаштування підключення до PostgreSQL
var connectionString = "Host=localhost;Database=FishDb;Username=postgres;Password=123";
builder.Services.AddDbContext<FishContext>(options =>
    options.UseNpgsql(connectionString));

// Додавання контролерів та Views
builder.Services.AddControllersWithViews();

// Додавання Razor Pages (для scaffolding)
builder.Services.AddRazorPages();

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();

// Маршрути для MVC контролерів
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

// Маршрути для Razor Pages
app.MapRazorPages();

app.Run();
