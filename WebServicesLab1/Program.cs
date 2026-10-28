using WebServicesLab1.Services;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddTransient<IEmailSender, EmailService>();

builder.Services.AddLocalization(o => o.ResourcesPath = "Resources");

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(o =>
{
    var cultures = new[]
    {
        new CultureInfo("uk"),
        new CultureInfo("en")
    };

    o.DefaultRequestCulture = new RequestCulture("uk");
    o.SupportedCultures = cultures;
    o.SupportedUICultures = cultures;
});

var app = builder.Build();

app.UseRequestLocalization();

app.Use(async (context, next) =>
{
    var requestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
    var fullUrl = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
    var logMessage = $"[{requestTime}] IP: {ipAddress} | URL: {fullUrl}\n";
    
    var logDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    if (!Directory.Exists(logDirectory))
    {
        Directory.CreateDirectory(logDirectory);
    }
    
    var logFilePath = Path.Combine(logDirectory, $"log-{DateTime.Now:yyyy-MM-dd}.txt");
    await File.AppendAllTextAsync(logFilePath, logMessage);
    
    await next.Invoke();
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();