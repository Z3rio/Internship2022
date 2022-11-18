//using SlackNet;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.DependencyInjection;
//using SlackNet.AspNetCore;
//using SlackNet.WebApi;
//using SlackNet.Blocks;
//using SlackNet.Events;
//using SlackNet.Extensions.DependencyInjection;
//using SlackNet.Interaction;

//namespace WebApplication1.Bots
//{
//    public class EchoDemo : ISlashCommandHandler
//    {
//        public async Task<SlashCommandResponse> Handle(SlashCommand command)
//        {
//            Console.WriteLine($"{command.UserName} used the {SlashCommand} slash command in the {command.ChannelName} channel");

//            return new SlashCommandResponse
//            {
//                Message = new Message
//                {
//                    Text = command.Text
//                }
//            };
//        }
//    }

//    public class Slack
//    {
//        private ISlackApiClient? api;
//        public static Task Start() => new Slack().MainAsync();

//        public Task MainAsync()
//        {
//            api = new SlackServiceBuilder()
//             .UseApiToken(Program.slackToken)
//             .RegisterSlashCommandHandler("/echo", EchoDemo);

//            return Task.CompletedTask;
//        }
//    }
//}
