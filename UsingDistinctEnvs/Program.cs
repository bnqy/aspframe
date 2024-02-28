var builder = WebApplication.CreateBuilder(args);
IHostEnvironment env = builder.Environment;

builder.Services.AddProblemDetails(); // for exception handler middlw

builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: false)
	.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

if (env.IsDevelopment())
{
	builder.Configuration.AddUserSecrets<Program>();  // in production and staging the usersecretds will not be used
}


var app = builder.Build();

if (!builder.Environment.IsDevelopment())
{
	app.UseExceptionHandler();  // when not in development it uses ExcepHandMiddl
}

app.MapGet("/", (IConfiguration i) => i.AsEnumerable());

app.Run();
