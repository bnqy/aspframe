var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear(); // clears default providers WAB
builder.Configuration.AddJsonFile("appsettings.json", optional: true);  // adds JSON rovider for appsettings.json

var zoomLevel = builder.Configuration["MappSettings:DefaultZoomLevel"];  // can retrieve any value by key with dict syntax
var lat = builder.Configuration["MappSettings:DefaultLocation:latitude"];  // gets latitude from appsettings
																		   // If the requested configuration key doesn’t exist, you get a null value.
var lat1 = builder.Configuration.GetSection("MappSettings")["DefaultLocation:latitude"];  // another way to get latitude from json file


var app = builder.Build();

app.MapGet("/", () => app.Configuration.AsEnumerable());  // returns key-values
//app.MapGet("/", (IConfiguration ic) => ic.AsEnumerable());  // returns key-values same as above we can inject IConfigaration

app.Run();
