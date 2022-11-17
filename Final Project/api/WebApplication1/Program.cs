using System.Runtime.CompilerServices;
using WebApplication1.DotEnv;

namespace WebApplication1
{
    public static class Program {

        public static IConfiguration? cfg = null;

        public static string? apiKey;
        public static string? discordToken;

        public static void Main(string[] args)
        {
            var root = Directory.GetCurrentDirectory();
            var dotenv = Path.Combine(root, ".env");
            DotEnv.DotEnv.Load(dotenv);

            Program.cfg = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            if (Program.cfg["GOOGLE_API_KEY"] != null)
            {
                Program.apiKey = Program.cfg["GOOGLE_API_KEY"];
            }
            if (Program.cfg["DISCORD_BOT_TOKEN"] != null)
            {
                Program.discordToken = Program.cfg["DISCORD_BOT_TOKEN"];
            }

            Bots.Discord.Start();


            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (app.Environment.IsDevelopment() == false)
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
                pattern: "{controller=Home}/{action=Index}"
            );

            app.Run();
        }    
    }
}