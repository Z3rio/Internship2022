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
        private DiscordSocketClient? client;
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

            var ROTCommand = new SlashCommandBuilder();
            ROTCommand.WithName("resturantsoftoday");
            ROTCommand.WithDescription("See the resturants of today");

            if (client != null)
            {
                try
                {
                    await client.CreateGlobalApplicationCommandAsync(searchCommand.Build());
                    await client.CreateGlobalApplicationCommandAsync(ROTCommand.Build());
                }
                catch (HttpException exception)
                {
                    var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);
                    Console.WriteLine(json);
                }
            }
        }

        private async Task SlashCommandHandler(SocketSlashCommand command)
        {   
            if (command != null)
            {
                switch (command.Data.Name)
                {
                    case "resturantsoftoday":
                        string? apiresp2 = await APICallAsync("https://localhost:7115/resturants/resturantsoftheday", "", "application/json");

                        if (apiresp2 != null)
                        {
                            PlaceObj[] apiobj2 = JsonConvert.DeserializeObject<PlaceObj[]>(apiresp2);

                            if (apiobj2 != null)
                            {
                                var embedBuiler = new EmbedBuilder()
                                    .WithAuthor(command.User.Username.ToString(), command.User.GetAvatarUrl() ?? command.User.GetDefaultAvatarUrl())
                                    .WithTitle("Resturants of today")
                                    .WithDescription($"There are {apiobj2.Count()} resturants of today! 🥳")
                                    .WithColor(Color.Green)
                                    .WithCurrentTimestamp();

                                for (int i = 0; i < apiobj2.Count(); i++)
                                {
                                    PlaceObj data = apiobj2[i];

                                    if (data.permanently_closed != true && data.opening_hours != null)
                                    {
                                        string OpenStr = data.opening_hours.open_now == true ? "✅" : "❌";
                                        embedBuiler.AddField(data.name, $"{OpenStr} {data.vicinity}", true);
                                    }
                                }

                                await command.RespondAsync(embed: embedBuiler.Build());
                            }
                        }
                        break;
                    case "search":
                        var keywordObj = command.Data.Options.Where(s => s.Name == "keyword");
                        var radiusObj = command.Data.Options.Where(s => s.Name == "radius");

                        if (keywordObj != null && radiusObj != null)
                        {
                            string? keyword = keywordObj.First().Value.ToString();
                            int? radius = int.Parse(radiusObj.First().Value.ToString());

                            if (keyword != null && radius != null)
                            {
                                if (radius < 500 || radius > 2500)
                                {
                                    await command.RespondAsync("the radius has to be between 500 and 2500");
                                }
                                else
                                {
                                    string? apiresp = await APICallAsync("https://localhost:7115/resturants/search", $"?search={keyword}&radius={radius}&sort=opennow", "application/json");

                                    if (apiresp != null)
                                    {
                                        UnsortedResults? apiobj = JsonConvert.DeserializeObject<UnsortedResults>(apiresp);

                                        if (apiobj != null && apiobj.results != null)
                                        {
                                            var embedBuiler = new EmbedBuilder()
                                            .WithAuthor(command.User.Username.ToString(), command.User.GetAvatarUrl() ?? command.User.GetDefaultAvatarUrl())
                                            .WithTitle("Resturant searching")
                                            .WithDescription($"You searched for \"{keyword}\" and got {apiobj.results.Count()}x results")
                                            .WithColor(Color.Green)
                                            .WithCurrentTimestamp();

                                            for (int i = 0; i < apiobj.results.Count(); i++)
                                            {
                                                PlaceObj data = apiobj.results[i];

                                                if (data.permanently_closed != true && data.opening_hours != null)
                                                {
                                                    string OpenStr = data.opening_hours.open_now == true ? "✅" : "❌";
                                                    embedBuiler.AddField(data.name, $"{OpenStr} {data.vicinity}", true);
                                                }
                                            }

                                            await command.RespondAsync(embed: embedBuiler.Build());
                                        }
                                    }
                                }
                            }
                            else
                            {
                                await command.RespondAsync("you have to input both a keyword and a radius");
                            }
                        }
                        break;
                }
            }
        }
    }
}
