using HallOfFame.Data;
using HallOfFame.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Ошибка: Строка подключения 'DefaultConnection' не найдена в конфигурации.");
}
else
{
    builder.Services.AddDbContext<HallOfFameDbContext>(options =>
        options.UseNpgsql(connectionString));
}

builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers(); 

app.Run();