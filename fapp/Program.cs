using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using fapp;

var people = new List<Person>
{
 new("Tom", "Hanks"),
 new("Denzel", "Washington"),
 new("Leondardo", "DiCaprio"),
 new("Al", "Pacino"),
 new("Morgan", "Freeman"),
};

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(opts => opts.LoggingFields = HttpLoggingFields.RequestProperties);

builder.Logging.AddFilter("Microsoft.AspNetCore.HttpLogging", LogLevel.Information);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
	app.UseHttpLogging();
}

app.MapGet("/", () => "Hello World!");
//app.MapGet("/person", () => new List<Person> { new ("Dua", "Lipa"), new("Bektur", "Omurkan")});
//app.MapGet("/adam", () => new Adam { code = "hY8wjjh", id = 789});
//app.MapGet("/person/{name}", (string name) => people.Where(p => p.FirstName.StartsWith(name)));

app.Run();