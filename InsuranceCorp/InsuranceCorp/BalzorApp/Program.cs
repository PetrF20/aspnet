using BalzorApp.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>(); //mo�nost p�id�n� vlastn� service - je pro v�echny u�ivatele jedna aplikace

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) //dotaz, jestli jsme v PROD prost�ed� - nebo v DEV prost�ed�
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub(); //u MVC bylo mapov�n� controller�, mapov�n� BlazerHub

app.MapFallbackToPage("/_Host");

app.Run();
