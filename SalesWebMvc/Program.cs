﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SalesWebMvc.Data;
using System.Configuration;
using SalesWebMvc.Models;
using SalesWebMvc.Services;
using System.Globalization;
using Microsoft.AspNetCore.Localization;


var builder = WebApplication.CreateBuilder(args);
var ConnectionString = "server=localhost;userid=developer;password=1234;database=saleswebmvcappdb";
builder.Services.AddDbContext<SalesWebMvcContext>(options =>
    options.UseMySql(ConnectionString, ServerVersion.AutoDetect(ConnectionString)));

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionStringMysql = builder.Configuration.GetConnectionString("SalesWebMvcContext");
builder.Services.AddDbContext<SalesWebMvcContext>(options => options.UseMySql(connectionStringMysql, ServerVersion.Parse("8.0.35-mysql")));

builder.Services.AddScoped<SeedingService>();
builder.Services.AddScoped<SellerService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<SalesRecordsService>();

var app = builder.Build();

var enuS = new CultureInfo("en-US");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(enuS),
    SupportedCultures = new List<CultureInfo> { enuS },
    SupportedUICultures = new List<CultureInfo> { enuS }
};
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.Services.CreateScope().ServiceProvider.GetRequiredService<SeedingService>().Seed();


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
