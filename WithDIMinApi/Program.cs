using WithDIMinApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // adds all services of Razor to the IServiceCollection

builder.Services.AddScoped<IEmailSender, EmailSender>();  // Whenenver we require a IEmailSender use EmailSender
builder.Services.AddSingleton<NetworkClient>(); // Whenever we require a NetworkClient use NetworkClient
builder.Services.AddScoped<MessageFactory>(); // Whenever we require a MessageFactory use MessageFactory
builder.Services.AddSingleton(provider => // review
	new EmailServerSettings
	(
		Host: "smtp.server.com",
		Port: 25
	));

var app = builder.Build();

LinkGenerator generator = app.Services.GetRequiredService<LinkGenerator>(); // retrieve service from DI using WebApplication.Services

app.MapGet("/", () => "Hello World!");
app.MapGet("/register/{username}", RegisterUser).WithName("Email");
app.MapRazorPages();  // registers all Razors as an endpoint

app.MapGet("/links", (LinkGenerator generator) =>    // injects service in endpoint handler 
{
	string link = generator.GetPathByName("Email", new { username = "dualipa"});
	return $"View email sender at {link}";
});

app.Run();

string RegisterUser(string username, IEmailSender emailSender)  // it depends on IEmailSender not specific EmailSender
{
	emailSender.SendEmail(username);
	return $"Email sent to {username}";
}
