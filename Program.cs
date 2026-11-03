
using Microsoft.EntityFrameworkCore;
using Costin_Catalina_Lab2.Models;
using Costin_Catalina_Lab2.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Conexiunea existenta la baza de date
var connectionString = builder.Configuration.GetConnectionString(
    "Costin_Catalina_Lab2Context")
    ?? throw new InvalidOperationException(
        "Connection string not found.");

// Contextul pentru biblioteca
builder.Services.AddDbContext<Costin_Catalina_Lab2Context>(
    options => options.UseSqlServer(connectionString));

// Contextul pentru autentificare (Identity)
builder.Services.AddDbContext<ApplicationDbContext>(
    options => options.UseSqlServer(connectionString));

// Configurarea Identity
builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// Autentificarea utilizatorilor
app.UseAuthentication();

// Autorizarea utilizatorilor
app.UseAuthorization();

app.MapStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
