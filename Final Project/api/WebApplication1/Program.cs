using System.Runtime.CompilerServices;
using WebApplication1.DotEnv;

namespace WebApplication1
{
    public static class Program {

        public static IConfiguration? cfg = null;

        public static string? apiKey;
        public static string? discordToken;
        public static string? slackToken;
        public static string? slackSigningSecret;
        public static WebApplicationBuilder ? builder;

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

                Bots.Discord.Start();
            }
            //if (Program.cfg["SLACK_BOT_OATH_TOKEN"] != null && Program.cfg["SLACK_SIGNING_SECRET"] != null)
            //{
            //    Program.slackToken = Program.cfg["SLACK_BOT_OATH_TOKEN"];
            //    Program.slackSigningSecret = Program.cfg["SLACK_SIGNING_SECRET"];

            //    Bots.Slack.Start();
            //}

            builder = WebApplication.CreateBuilder(args);

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