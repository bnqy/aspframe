using Microsoft.AspNetCore.Diagnostics;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();


if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/error");
}
app.UseWelcomePage("/");
app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.UseRouting();

app.MapGet("/error", () => "Sorry, an error occurred");
app.MapGet("/", () => "Hello world!");


app.Run();
