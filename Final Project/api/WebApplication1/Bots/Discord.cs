using Discord.WebSocket;
using Discord;
using Discord.Net;
using Newtonsoft.Json;
using static api.Handler;
using WebApplication1.Models;
using static WebApplication1.Models.ResturantsModel;

namespace WebApplication1.Bots
{
    public class Discord
    {
        private DiscordSocketClient client;
        private ulong guildId = 931629164656734238;

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        public static Task Start() => new Discord().MainAsync();

        public async Task MainAsync()
        {
            DiscordSocketConfig config = new()
            {
                UseInteractionSnowflakeDate = false
            };

            client = new DiscordSocketClient(config);

            client.Log += Log;

            client.Ready += Client_Ready;

            client.SlashCommandExecuted += SlashCommandHandler;

            await client.LoginAsync(TokenType.Bot, Program.discordToken);
            await client.StartAsync();

            await Task.Delay(-1);
        }

        public async Task Client_Ready()
        {
            var searchCommand = new SlashCommandBuilder();
            searchCommand.WithName("search");
            searchCommand.WithDescription("Search for an resturant in the area");
            searchCommand.AddOption("keyword", ApplicationCommandOptionType.String, "The keyword to search for", isRequired: true);
            searchCommand.AddOption("radius", ApplicationCommandOptionType.Number, "The radius to search within (500-2500 meters)", isRequired: true);

            try
            {
                await client.CreateGlobalApplicationCommandAsync(searchCommand.Build());
            }
            catch (ApplicationCommandException exception)
            {
                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                Console.WriteLine(json);
            }
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {   
            if (command != null)
            {
                switch (command.Data.Name)
                {
                    case "search":
                        var keywordObj = from s in command.Data.Options
                                     where s.Name == "keyword"
                                     select s;
                        var radiusObj = from s in command.Data.Options
                                         where s.Name == "radius"
                                         select s;

                        string keyword = keywordObj.First().Value.ToString();
                        int radius = int.Parse(radiusObj.First().Value.ToString());

                        if (keyword != null && radius != null)
                        {
                            if (radius < 500 || radius > 2500)
                            {
                                await command.RespondAsync("the radius has to be between 500 and 2500");
                            }
                            else
                            {
                                string apiresp = await APICallAsync("https://localhost:7115/resturants/search", $"?search={keyword}&radius={radius}&sort=opennow", "application/json");

                                if (apiresp != null)
                                {
                                    UnsortedResults apiobj = JsonConvert.DeserializeObject<UnsortedResults>(apiresp);

                                    var embedBuiler = new EmbedBuilder()
                                        .WithAuthor(command.User.Username.ToString(), command.User.GetAvatarUrl() ?? command.User.GetDefaultAvatarUrl())
                                        .WithTitle("Resturants")
                                        .WithDescription($"You searched for {keyword} and got {apiobj.results.Count()}x results")
                                        .WithColor(Color.Green)
                                        .WithCurrentTimestamp();

                                    for (int i = 0; i < apiobj.results.Count(); i++)
                                    {
                                        PlaceObj data = apiobj.results[i];

                                        if (data.permanently_closed != true)
                                        {
                                            string OpenStr = data.opening_hours.open_now == true ? "✅" : "❌";
                                            embedBuiler.AddField(data.name, $"{OpenStr} {data.vicinity}", true);
                                        }
                                    }

                                    await command.RespondAsync(embed: embedBuiler.Build());
                                }
                            }
                        }
                        else
                        {
                            await command.RespondAsync("you have to input both a keyword and a radius");
                        }

                        break;
                }
            }
        }
    }
}
