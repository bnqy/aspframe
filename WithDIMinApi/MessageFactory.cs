namespace WithDIMinApi
{
	public class MessageFactory
	{
		public Email Create(string emailAddress)
			=> new Email(emailAddress, "Thanks for signing up!");
	}
}
