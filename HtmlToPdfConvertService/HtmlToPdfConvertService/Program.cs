using HtmlToPdfConvertService.Models;
using HtmlToPdfConvertService.Models.Impl;
using HtmlToPdfConvertService.Models.Items;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connection = builder.Configuration.GetConnectionString("ItemsDbConnection");
builder.Services.AddDbContext<ItemsDbContext>(options => options.UseSqlServer(connection), ServiceLifetime.Singleton);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IItemManager, ItemManager>();
builder.Services.AddSingleton<IItemsRepository, ItemsDbRepository>();
builder.Services.AddSingleton<IHtmlToPdfConverter, PuppeteerSharpConverter>();

var app = builder.Build();

app.UseExceptionHandler("/Home/Error");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
