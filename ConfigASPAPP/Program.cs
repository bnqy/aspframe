using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.Sources.Clear(); // clears default providers WAB
builder.Configuration.AddJsonFile("sharedSettings.json", optional: true); // adds sharedSettings before appsettinngs.json
builder.Configuration.AddJsonFile("appsettings.json", optional: true, // adds JSON rovider for appsettings.json  // appsettings added after sharedSettings so it will overrite same keys
	reloadOnChange: true);   //IConfiguration will be rebuilt if appsettings.json file changes
builder.Configuration.AddEnvironmentVariables();  // adds machines env variables as conf provider  // this will overrite same keys in above


builder.Services.Configure<AppDisplaySettings>(builder.Configuration.GetSection("AppDisplaySettings")); // binds the AppDisplaySettings section to POCO option class AppDisplaySettings
builder.Services.Configure<MappSettings>(builder.Configuration.GetSection(nameof(MappSettings)));
builder.Services.Configure<List<Store>>(builder.Configuration.GetSection("Stores"));  //List<Store>


var zoomLevel = builder.Configuration["MappSettings:DefaultZoomLevel"];  // can retrieve any value by key with dict syntax
var lat = builder.Configuration["MappSettings:DefaultLocation:latitude"];  // gets latitude from appsettings
																		   // If the requested configuration key doesn’t exist, you get a null value.
var lat1 = builder.Configuration.GetSection("MappSettings")["DefaultLocation:latitude"];  // another way to get latitude from json file
// returns IConfigrationSection

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet("/ex", () => app.Configuration.AsEnumerable());  // returns key-values
														  //app.MapGet("/", (IConfiguration ic) => ic.AsEnumerable());  // returns key-values same as above we can inject IConfigaration
app.MapGet("/ex2", () => $"{zoomLevel}, {lat}, {lat1}");

app.MapGet("/display-settings", (IConfiguration ic) =>
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
	string title = ic["AppDisplaySettings:Title"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8604 // Possible null reference argument.
	bool showCopyright = bool.Parse(ic["AppDisplaySettings:ShowCopyright"]);
#pragma warning restore CS8604 // Possible null reference argument.

	return new {title, showCopyright};
});


// to get work the POCO class has to be registered Configure<T>
app.MapGet("/display-settings2", (IOptions<AppDisplaySettings> options, IOptions<MappSettings> options2) =>  // strongly typed with IOptions
{
	AppDisplaySettings settings = options.Value;  // Value exposes POCOs

	string title = settings.Title;
	bool showCopyright = settings.ShowCopyright;  // binder can also convert to builtin types

	MappSettings settings2 = options2.Value;
	int defaultZoomLevel = settings2.DefaultZoomLevel;
	Location defloc = settings2.DefaultLocation;

	return new { title, showCopyright, defaultZoomLevel, defloc};
});


// IOptionsSnapshot<T> updates autoly if the underlying file changes
app.MapGet("/display-settings3", (IOptionsSnapshot<AppDisplaySettings> iopsc) =>
{
	AppDisplaySettings settings = iopsc.Value;

	return new
	{
		title = settings.Title,
		showCopyright = settings.ShowCopyright
	};
});

app.MapGet("/mapsettings",(IOptions<MappSettings> opt) => opt.Value);
app.MapGet("/displaysettings", (IOptionsSnapshot<AppDisplaySettings> opt) => opt.Value);
app.MapGet("/stores", (IOptions<List<Store>> opt) => opt.Value);

//donot favor following
/*app.MapGet("/display-settings", (IConfiguration ic) =>
{
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
	string title = ic["AppDisplaySettings:Title"];
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

#pragma warning disable CS8604 // Possible null reference argument.
	bool showCopyright = bool.Parse(ic["AppDisplaySettings:ShowCopyright"]);
#pragma warning restore CS8604 // Possible null reference argument.

	return new { title, showCopyright };
});*/

app.Run();


public class AppDisplaySettings
{
	public string? Title { get; set; }
	public bool ShowCopyright { get; set; }
}

public class MappSettings
{
	public string GoogleMapsApiKey { get; set; }
	public int DefaultZoomLevel { get; set; }

    public Location DefaultLocation { get; set; }
}

public class Location
{
	public decimal Latitude { get; set; }
	public decimal Longitude { get; set; }
}

public class Store
{
	public string Name { get; set; }
	public Location Location { get; set; }
}