using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using UserManagement.Data;
using UserManagement.Repositories;
using UserManagement.Repositories.Interfaces;
using UserManagement.Services;
using UserManagement.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Register IDbConnection for Dapper
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = builder.Configuration.GetConnectionString("EmployeeDbConnection");
    return new SqliteConnection(connectionString);
});
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IRoleRepository,RoleRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<SchemaInitializer>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "Issuer",
            ValidAudience = "Audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKey"))
        };
    });

builder.Services.AddControllers();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Run the schema initializer
using (var scope = app.Services.CreateScope())
{
    var schemaInitializer = scope.ServiceProvider.GetRequiredService<SchemaInitializer>();
    schemaInitializer.Initialize();
}
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

app.Run();
