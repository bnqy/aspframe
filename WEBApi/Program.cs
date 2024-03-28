using Microsoft.AspNetCore.Mvc.Formatters;
using WEBApi.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<FruitService>();

builder.Services.AddRazorPages() // it doesnot let you pass lambda opt so use AddMvcOpt()
	.AddMvcOptions(opt =>
	{
		opt.Filters.Add(new LogResourceFilter());  // adds filters to rp
		//opt.Filters.Add<LogResourceFilter>();
		//opt.Filters.Add(typeof(LogResourceFilter));

	});

builder.Services.AddControllers(opt =>
{
	//opt.RespectBrowserAcceptHeader = true;
	//opt.OutputFormatters.RemoveType<StringOutputFormatter>();

	opt.Filters.Add<LogResourceFilter>();  // adds filter glabally
										   //opt.Filters.Add(typeof(LogResourceFilter)); // same as above
										   //opt.Filters.Add(new LogResourceFilter()); // same


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