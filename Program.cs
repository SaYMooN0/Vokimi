using Vokimi.Services.Classes;
using DotNetEnv;
using Dapper;
using Vokimi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;

internal class Program
{
    private static void Main(string[] args)
    {
        SqlMapper.AddTypeHandler(new DateTypesHandler());
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();


        Logger _logger = new Logger();
        builder.Services.AddSingleton<VokimiServices.ILogger, Logger>(provider => _logger);

        Env.Load();
        string databaseConnectionString = Environment.GetEnvironmentVariable("DATABASE_CONNECTION_STRING");
        if (string.IsNullOrEmpty(databaseConnectionString))
        {
            _logger.CriticalError("DatabaseConnectionString is null or empty");
            return;
        }
        DataBase _db = new DataBase(databaseConnectionString);
        builder.Services.AddSingleton<VokimiServices.IDataBase, DataBase>(provider => _db);

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options => { options.LoginPath = new PathString("/Account/Authorization"); });

        builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromDays(14));

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseSession();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}
