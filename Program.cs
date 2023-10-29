using Vokimi.Services.Classes;
using DotNetEnv;
using Dapper;
using Vokimi.Models;

internal class Program
{
    private static async Task Main(string[] args)
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

        var app = builder.Build();

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
    }
}
