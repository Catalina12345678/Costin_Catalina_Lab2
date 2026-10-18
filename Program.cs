
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString(
    "Costin_Catalina_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string not found.");

builder.Services.AddDbContext<Costin_Catalina_Lab2Context>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
