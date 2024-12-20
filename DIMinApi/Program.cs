var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/register/{username}", RegisterUser);
app.MapGet("/", () => "Navigate to register/{username} to test the Mock email sending");

using var scope = app.Services.CreateAsyncScope();
var linkGenerator = scope.ServiceProvider
	.GetRequiredService<LinkGenerator>();

app.Run();


string RegisterUser(string username)
{
	var emailSender = new EmailSender(  // to create EmailSender you have to create all its depencies
				new MessageFactory(), 
				new NetworkClient(  // NetworkClient also needs a dependencies
					new EmailServerSettings(Host: "smtp.server.com", Port: 25
					)));
	emailSender.SendEmail(username);
	return $"Email sent to {username}!";
}

public record Email(string Address, string Message);
public record EmailServerSettings(string Host, int Port);

public class EmailSender
{
	private readonly NetworkClient _client;
	private readonly MessageFactory _factory;   // EmailSender depends on two other classes

	public EmailSender(MessageFactory factory, NetworkClient client)
	{
		_factory = factory;
		_client = client;
	}

	public void SendEmail(string username)
	{
		var email = _factory.Create(username);
		_client.SendEmail(email);
		Console.WriteLine($"Email sent to {username}!");
	}
}

public class NetworkClient
{
	private readonly EmailServerSettings _settings;

	public NetworkClient(EmailServerSettings settings)
	{
		_settings = settings;
	}

	public void SendEmail(Email email)
	{
		Console.WriteLine($"Connecting to server {_settings.Host}:{_settings.Port}");
		Console.WriteLine($"Email sent to {email.Address}: {email.Message}");
	}
}

public class MessageFactory
{
	public Email Create(string emailAddress)
		=> new Email(emailAddress, "Thanks for signing up!");
}