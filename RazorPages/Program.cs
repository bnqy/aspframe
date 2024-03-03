var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();     // registers Razor Pages

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();  // sets security headers
}

app.UseHttpsRedirection();  // ensures to use only https
app.UseStaticFiles();

app.UseRouting();  // responsible for selecting endpoint for incoming request

app.UseAuthorization();

app.MapRazorPages();  // register each Razor Pages as an endpoint

app.Run();
