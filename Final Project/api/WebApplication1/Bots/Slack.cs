using SlackNet;
using System.Runtime.CompilerServices;

namespace WebApplication1.Bots
{
    public class Slack
    {
        private ISlackApiClient? api;
        public static Task Start() => new Slack().MainAsync();

        public async Task MainAsync()
        {
            api = new SlackServiceBuilder()
             .UseApiToken(Program.slackToken)
             .GetApiClient();
        }
    }
}
