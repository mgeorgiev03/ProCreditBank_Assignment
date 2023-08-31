using DataAccess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
