using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBSApp.Logger
{
    public interface ILogger
    {
        public Task Log(LogMessage message);
    }
}
