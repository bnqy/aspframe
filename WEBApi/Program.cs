using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<FruitService>();


builder.Services.AddControllers(opt =>
{
	//opt.RespectBrowserAcceptHeader = true;
	//opt.OutputFormatters.RemoveType<StringOutputFormatter>();
}) // adds necassery services for controllers
	.AddXmlSerializerFormatters()  // adds XML formatter
	.AddNewtonsoftJson()  // adds JSON formatter (newton rather then text.json)
    .ConfigureApiBehaviorOptions(opt =>
	{
		//opt.SuppressModelStateInvalidFilter = true; // this will ignore automatic 400 bad request for invalid request
	});



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();  // related with Swagger
builder.Services.AddSwaggerGen();  // this is too

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();  // adds actions as endpoints

app.Run();


public class FruitService
{
	public List<string> Fruit { get; } = new List<string>
	{
		"Pear",
		"Lemon",
		"Peach"
	};
}