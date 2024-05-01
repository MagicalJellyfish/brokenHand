using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.Interactions;

namespace brokenHand.Discord.Modules.Ping
{
    public class PingTextModule : ModuleBase<SocketCommandContext>
    {
        public PingTextModule() { }

        [Command("ping")]
        public Task Ping()
        {
            return ReplyAsync("Pong!");
        }
    }
}
