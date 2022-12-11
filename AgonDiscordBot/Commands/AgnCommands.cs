using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Threading.Tasks;

namespace AgonDiscordBot.Commands
{
    class AgnCommands : BaseCommandModule
    {
       
   
        private static CommandContext CommandContext()
        {
            throw new NotImplementedException();
        }

        [Command("mybot")]
        [Description("Bot Answer You")]
      //  [RequireRoles(RoleCheckMode.Any, "[MOD]")]
        public async Task HelloWorld(CommandContext ctx)
        {
           await ctx.Channel.SendMessageAsync("Yes, My Love").ConfigureAwait(false);
        }

        [Command("Sum")]
        [Description("Add two numbers together")]
        public async Task Sum(CommandContext ctx,
        [Description("First Number to Add")] int numberOne,
        [Description("Second Number to Add")] int numberTwo)
        {
            await ctx.Channel.SendMessageAsync($"The Sum Of {numberOne} and {numberTwo} is {numberOne + numberTwo}").ConfigureAwait(false);
            await ctx.Member.SendMessageAsync($"The Sum Of {numberOne} and {numberTwo} is {numberOne + numberTwo}").ConfigureAwait(false);
        }
        [Command("Profile")]
        public async Task Profile(CommandContext ctx, string member)
        {
            await GetProfileToDisplayAsync(ctx, member);
        }

        private async Task GetProfileToDisplayAsync(CommandContext ctx, string member)
        {
            
            var profilediscord = new DiscordEmbedBuilder()
            {
                Title = $" Tesst Title"
            };
            profilediscord.AddField("Name", member);

            await ctx.Channel.SendMessageAsync(embed: profilediscord).ConfigureAwait(false);
        }
    }
}
