using Blazr.App.Infrastructure;
using Blazr.App.UI;
using Blazr.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAppServerDataServices();
builder.Services.AddAppUIServices();

builder.Configuration.AddJsonFile("countries.json",
        optional: true,
        reloadOnChange: true);

builder.Services.Configure<List<CountryData>>(
    builder.Configuration.GetSection("Countries"));

var app = builder.Build();

// get the DbContext factory and add the test data
var factory = app.Services.GetService<IDbContextFactory<InMemoryWeatherDbContext>>();
if (factory is not null)
    WeatherTestDataProvider.Instance().LoadDbContext<InMemoryWeatherDbContext>(factory);

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
