using Gejms.Server.Data;
using Gejms.Server.Entities;
using Gejms.Server.Extensions;
using Gejms.Server.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var envDbUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
var dbURL = string.IsNullOrEmpty(envDbUrl) ?
    string.Format("Server={0};Port=5432;User Id={1};Password={2};Database={3}", "localhost", "postgres", "postgres", "gejms")
    :
    StringUtilities.ParseDbURI(envDbUrl);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

//setup db
builder.Services.AddDbContext<GejmsDbContext>(
    opts => opts.UseNpgsql(dbURL)
    );
builder.Services.AddScoped<GejmsDbContext, GejmsDbContext>();

// setup Identity
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.SignIn.RequireConfirmedPhoneNumber = false;
    opts.SignIn.RequireConfirmedEmail = false;
    opts.SignIn.RequireConfirmedPhoneNumber = false;
    opts.SignIn.RequireConfirmedAccount = false;

    opts.Password.RequireDigit = false;
    opts.Password.RequiredLength = 5;
    opts.Password.RequireLowercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequireUppercase = false;

    opts.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
}).AddEntityFrameworkStores<GejmsDbContext>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "GejmsCookie";
    config.Cookie.HttpOnly = true;
    
    config.LoginPath = "/account/Login";
    config.LogoutPath = "/account/Logout";

    config.ExpireTimeSpan = TimeSpan.FromDays(30);
    config.SlidingExpiration = true;

    config.Events.OnRedirectToLogin = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    config.Events.OnRedirectToAccessDenied = ctx =>
    {
        ctx.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.MigrateDatabase();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
