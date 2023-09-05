using DataAccess;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseContext>();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
