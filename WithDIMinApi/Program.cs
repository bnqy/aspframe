using WithDIMinApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages(); // adds all services of Razor to the IServiceCollection
builder.Services.AddScoped<IEmailSender, EmailSender>();  // review
builder.Services.AddSingleton<NetworkClient>(); // review
builder.Services.AddScoped<MessageFactory>(); // review
builder.Services.AddSingleton(provider => // review
	new EmailServerSettings
	(
		Host: "smtp.server.com",
		Port: 25
	));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/register/{username}", RegisterUser);
app.MapRazorPages();  // registers all Razors as an endpoint


app.Run();

string RegisterUser(string username, IEmailSender emailSender)  // it depends on IEmailSender not specific EmailSender
{
	emailSender.SendEmail(username);
	return $"Email sent to {username}";
}
