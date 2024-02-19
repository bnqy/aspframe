namespace WithDIMinApi
{
	public static class EmailSenderServiceCollectionExtensions
	{
		public static IServiceCollection AddEmailSender(this IServiceCollection services)   // crates extention method for IServiceCollection
		{
			services.AddScoped<IEmailSender, EmailSender>();  // Whenenver we require a IEmailSender use EmailSender
			services.AddSingleton<NetworkClient>(); // Whenever we require a NetworkClient use NetworkClient
			services.AddScoped<MessageFactory>(); // Whenever we require a MessageFactory use MessageFactory

			services.AddScoped(provider => // IServiceProvider instanse
				new EmailServerSettings  // this instance of EmailServerSettings will be used whenever an instance is requiered
				(
					Host: "smtp.server.com",   // EmailServerSettings constr is created everytime when EmailServerSettings is required
					Port: 25
				));

			return services; // returns IServiceCollection by convention to method chaining
		}
	}
}
