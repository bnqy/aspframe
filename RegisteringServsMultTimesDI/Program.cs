var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IMessageSender, EmailSender>();  // registers in the DI container
builder.Services.AddScoped<IMessageSender, FacebookSender>();  // registers in the DI
builder.Services.AddScoped<IMessageSender, SmsSender>(); // adds to the DI container

var app = builder.Build();

app.MapGet("/", () => "Try calling /single-message/{username} or /multi-message/{username} and check the logs");
app.MapGet("/single-message/{username}", SendingSingleMessage);
app.MapGet("/multi-message/{username}", SendingMultiMessage);

app.Run();

string SendingSingleMessage(string username, IMessageSender sender)
{
	sender.SendMessage($"Hello, {username}!");
	return "Check the application logs to see what was called";
}

string SendingMultiMessage(string username, IEnumerable<IMessageSender> senders) // must use IEnumerable<T> to inject all servs
{
	foreach (var sender in senders)
	{
		sender.SendMessage($"Hello, {username}!");
	}

	return "Check the application logs to see what was called";
}

public interface IMessageSender
{
	public void SendMessage(string message);
}

public class EmailSender
	: IMessageSender
{
	public void SendMessage(string message)
	{
		Console.WriteLine($"Sending Email message: {message}");
	}
}

public class FacebookSender
	: IMessageSender
{
	public void SendMessage(string message)
	{
		Console.WriteLine($"Sending Facebook message: {message}");
	}
}

public class SmsSender
	: IMessageSender
{
	public void SendMessage(string message)
	{
		Console.WriteLine($"Sending SMS message: {message}");
	}
}