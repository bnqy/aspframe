using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapPost("/users", (UserModel model) => $"{model.ToString()}")
	.WithParameterValidation();

app.MapPost("user/{id}", ([AsParameters] GetUserModel model) => $"{model.Id.ToString()}")
	.WithParameterValidation();
//it does not work
// app.MapGet("/user/{id}", ([Range(0, 10)] int id) => id.ToString())
//    .WithParameterValidation();

app.Run();

public record UserModel : IValidatableObject
{
	[Required]
	[StringLength(100)]
	[Display(Name = "Your name")]
	public string Name { get; set; }

	[Required]
	[StringLength(100)]
	[Display(Name = "Last name")]
	public string LastName { get; set; }

	[EmailAddress]
	public string Email { get; set; }

	[Phone]
	[Display(Name = "Phone number")]
	public string PhoneNumber { get; set; }

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (string.IsNullOrEmpty(Email)
			&& string.IsNullOrEmpty(PhoneNumber))
		{
			yield return new ValidationResult(
				"You must provide either an Email or a PhoneNumber",
				new[] { nameof(Email), nameof(PhoneNumber) });
		}
	}
}

struct GetUserModel
{
	[Range(1, 10)]
	public int Id { get; set; }
}