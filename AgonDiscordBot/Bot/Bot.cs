using AgonDiscordBot.Commands;
using AgonDiscordBot.DonationAlert;
using AgonDiscordBot.DonationAlert.Structures;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AgonDiscordBot.Bot
{
    public class Bot
    {
        public DiscordClient Client { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
        public async Task DonateNotification(string donator_name,string amount, string date_created, DonationData donationProgress)
        {
            ulong channel_id = 938803271664025680;
            var channel = await Client.GetChannelAsync(channel_id);
            var profilediscord = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Cyan,
                Title = $"Donate Receipt"
                
            };

            profilediscord.AddField("Name", donator_name)
        .WithColor(DiscordColor.Blue)
        .WithTitle("I overwrote \"Hello world!\"")
        .WithDescription("I am a description.")
        .WithUrl("https://example.com");

            profilediscord.AddField("Amount", amount).WithColor(DiscordColor.Chartreuse);
            profilediscord.AddField("Donation date", date_created);
            profilediscord.AddField("Donation Progress", $"{donationProgress.currentprogress}/{donationProgress.DonateGoal} USD");

            await channel.SendMessageAsync(embed: profilediscord).ConfigureAwait(false);
        }
        private async Task<ConfigJson> ConfigurationFile()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);
            ConfigJson configJson = JsonConvert.DeserializeObject<ConfigJson>(json);
            return configJson;
        }

        public async Task StartBot()
        {

            var configJson = await ConfigurationFile();
            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug
            };
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableDms = false,
                EnableMentionPrefix = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);
            
            Commands.RegisterCommands<AgnCommands>();

            await Client.ConnectAsync();

            await Task.Delay(-1);
        }
        private Task OnClientReady(object sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
