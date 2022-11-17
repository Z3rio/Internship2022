using System.Runtime.CompilerServices;
using WebApplication1.DotEnv;

namespace WebApplication1
{
    public static class Program {

        public static IConfiguration cfg = null;

        public static string apiKey = "";
        public static void Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.DotEnv.Load(dotenv);

            Program.cfg = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();
            Program.apiKey = Program.cfg["GOOGLE_API_KEY"];

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() == false)
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}"
            );

            app.Run();
        }    
    }
}