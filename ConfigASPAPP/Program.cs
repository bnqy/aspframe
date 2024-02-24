var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear(); // clears default providers WAB
builder.Configuration.AddJsonFile("sharedSettings.json", optional: true); // adds sharedSettings before appsettinngs.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true);  // adds JSON rovider for appsettings.json  // appsettings added after sharedSettings so it will overrite same keys
builder.Configuration.AddEnvironmentVariables();  // adds machines env variables as conf provider  // this will overrite same keys in above

var zoomLevel = builder.Configuration["MappSettings:DefaultZoomLevel"];  // can retrieve any value by key with dict syntax
var lat = builder.Configuration["MappSettings:DefaultLocation:latitude"];  // gets latitude from appsettings
																		   // If the requested configuration key doesn’t exist, you get a null value.
var lat1 = builder.Configuration.GetSection("MappSettings")["DefaultLocation:latitude"];  // another way to get latitude from json file
// returns IConfigrationSection

var app = builder.Build();

app.MapGet("/", () => app.Configuration.AsEnumerable());  // returns key-values
														  //app.MapGet("/", (IConfiguration ic) => ic.AsEnumerable());  // returns key-values same as above we can inject IConfigaration
app.MapGet("/ex", () => $"{zoomLevel}, {lat}, {lat1}");

app.Run();
