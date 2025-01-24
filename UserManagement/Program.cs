using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UserManagement.Data;
using UserManagement.Helpers;
using UserManagement.Repositories;
using UserManagement.Repositories.Interfaces;
using UserManagement.Services;
using UserManagement.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Ensure the /data directory exists
// Read database settings from appsettings.json
var dbDirectory = builder.Configuration["DatabaseSettings:DbDirectory"];
var dbFileName = builder.Configuration["DatabaseSettings:DbFileName"];
var dbFullPath = Path.Combine(Directory.GetCurrentDirectory(), dbDirectory, dbFileName);

// Ensure the database directory exists
if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), dbDirectory)))
{
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), dbDirectory));
    Console.WriteLine($"Directory created: {dbDirectory}");
}
// Configure DbContext with SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"Data Source={dbFullPath}"));

// Register IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
{    
    return new SqliteConnection($"Data Source={dbFullPath}");
});
builder.Services.AddSingleton<JwtHelper>();

builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<AuthService>();


// JWT settings from appsettings.json
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});
// Add authorization
builder.Services.AddAuthorization();

builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Run the schema initializer
//using (var scope = app.Services.CreateScope())
//{
//    var schemaInitializer = scope.ServiceProvider.GetRequiredService<SchemaInitializer>();
//    schemaInitializer.Initialize();
//}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRequestLocalization();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    try
    {
        dbContext.Database.Migrate(); // Automatically applies all pending migrations
        Console.WriteLine("Database migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred while applying migrations: {ex.Message}");
    }
}


app.Run();
