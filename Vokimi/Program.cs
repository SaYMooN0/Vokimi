using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Vokimi.Components;
using Vokimi.Services;
using Vokimi.src.data;

namespace Vokimi
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            // Initialize database
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var appDbContext = services.GetRequiredService<VokimiDbContext>();
                    await DbInitializer.InitializeDbAsync(appDbContext);
                }
                catch (Exception ex)
                {
                    app.Logger.LogError(ex, "An error occurred while initializing the database.");
                    throw; // Re-throw the exception to stop the application start-up
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }


            app.UseAuthentication();
            app.UseAuthorization();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

         


            app.MapRazorComponents<App>()
               .AddInteractiveServerRenderMode()
               .AddInteractiveWebAssemblyRenderMode()
               .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddAntiforgery();

            // Database context configuration
            services.AddDbContext<VokimiDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("VokimiDb")));

            services.AddScoped<UsersDbOperationsService>();
            services.AddScoped<TestsDbOperationsService>();

            services.Configure<SmtpSettings>(configuration.GetSection("Smtp"));
            services.AddTransient<EmailService>();

            services.AddHttpContextAccessor();
            services.AddScoped<AuthHelperService>();

            services.AddAuthentication(AuthHelperService.AuthScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = AuthHelperService.AuthCookieName;
                    options.LoginPath = "/acc";
                    options.LogoutPath = "/logout";
                    options.AccessDeniedPath = "/access-denied";
                });
            services.AddAuthorization();
            services.AddCascadingAuthenticationState();

            services.AddRazorComponents()
                .AddInteractiveServerComponents()
                .AddInteractiveWebAssemblyComponents();

            services.AddSingleton<IAuthenticationSchemeProvider, AuthenticationSchemeProvider>();
        }
    }


}
