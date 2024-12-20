﻿namespace WithDIMinApi
{
	public class EmailSender : IEmailSender
	{
		private readonly NetworkClient _client;
		private readonly MessageFactory _factory;

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

}
