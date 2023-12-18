using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts => opts.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseHttpLogging();
}

app.MapGet("/", () => "Hello World!");
app.MapGet("/person", () => new List<Person> { new ("Dua", "Lipa"), new("Bektur", "Omurkan")});
app.MapGet("/adam", () => new Adam { code = "hY8wjjh", id = 789});

app.Run();


public record Person (string FistName, string LastName);

public class Adam
{
	public string? code { get; set; }
	public int id { get; set; }
}