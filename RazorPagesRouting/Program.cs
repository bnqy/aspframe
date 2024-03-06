using Microsoft.AspNetCore.Mvc.ApplicationModels;
using RazorPagesRouting;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(opts =>
{
    opts.Conventions.Add(
    new PageRouteTransformerConvention(
    new KebabCaseParameterTransformer()));
    opts.Conventions.AddPageRoute(
    "/Search/Products/StartSearch", "/search-products");
}); ;

builder.Services.AddSingleton<ProductService>();

// changes default routing adds slash at the end etc
builder.Services.Configure<RouteOptions>(options =>
{
	options.AppendTrailingSlash = true;
	options.LowercaseUrls = true;
	options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();


class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
   public string? TransformOutbound(object? value)
   {
        if (value is null)
        {
            return null;
       }

        return Regex.Replace(value.ToString()!, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}