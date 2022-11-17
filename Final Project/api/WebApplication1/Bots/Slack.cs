using SlackNet;

namespace WebApplication1.Bots
{
    public class Slack
    {
        private SlackApiClient? api;
        public static Task Start()
        {
            api = new SlackServiceBuilder()
                .UseApiToken(Program.slackToken)
                .GetApiClient();
        }
    }
}
