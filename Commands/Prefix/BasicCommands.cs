using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBoT.Commands.Prefix
{
    public class BasicCommands : BaseCommandModule
    {
        [Command("clear")]
        public async Task ClearChat(CommandContext ctx)
        {
            var user = ctx.Channel;


        }


    }
}
