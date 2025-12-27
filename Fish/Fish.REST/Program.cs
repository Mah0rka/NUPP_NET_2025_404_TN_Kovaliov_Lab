using Fish.Common.Services;
using Fish.Infrastructure;
using Fish.Infrastructure.Models;
using Fish.REST.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Налаштування підключення до PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Конвертація URI формату (postgresql://...) в ADO.NET формат для Npgsql
if (connectionString.StartsWith("postgresql://") || connectionString.StartsWith("postgres://"))
{
    var uri = new Uri(connectionString);
    var userInfo = uri.UserInfo.Split(':');
    var host = uri.Host;
    var port = uri.Port > 0 ? uri.Port : 5432;
    var database = uri.AbsolutePath.TrimStart('/');
    var username = userInfo[0];
    var password = userInfo.Length > 1 ? userInfo[1] : "";
    
    connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
    Console.WriteLine($"Connection string converted to ADO.NET format. Host: {host}, Database: {database}");
}

builder.Services.AddDbContext<FishContext>(options =>
    options.UseNpgsql(connectionString));

// Налаштування Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Налаштування паролів
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;

    // Налаштування користувача
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<FishContext>()
.AddDefaultTokenProviders();

// Налаштування JWT Authentication
var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"] 
    ?? throw new InvalidOperationException("JWT SecretKey not found.");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] 
    ?? throw new InvalidOperationException("JWT Issuer not found.");
var jwtAudience = builder.Configuration["JwtSettings:Audience"] 
    ?? throw new InvalidOperationException("JWT Audience not found.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});

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

// Налаштування Swagger/OpenAPI з підтримкою JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Fish REST API",
        Version = "v1",
        Description = "REST API для управління рибами та акваріумами з JWT аутентифікацією"
    });

    // Додавання JWT Bearer до Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Введіть JWT токен у форматі: Bearer {ваш токен}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Застосування міграцій та ініціалізація ролей при старті
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FishContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    
    try
    {
        await context.Database.MigrateAsync();
        Console.WriteLine("Міграції успішно застосовані");

        // Створення ролей
        string[] roles = { "Visitor", "Manager", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Роль {role} створена");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Помилка застосування міграцій: {ex.Message}");
    }
}

// Налаштування HTTP pipeline
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
