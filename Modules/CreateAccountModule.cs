using Discord;
using Discord.Interactions;
using DiscordBSApp.Logger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBSApp.Modules
{
    public class CreateAccountModule : InteractionModuleBase<SocketInteractionContext>
    {
        public InteractionService Commands { get; set; }

        private static Logger.Logger _logger;

        public CreateAccountModule(LoggerImpl logger)
        {
            _logger = logger;
        }

        [UserCommand("Mention")]
        public async Task MentionUser(IUser user)
        {
            await _logger.Log(new LogMessage(LogSeverity.Info, "CreateAccountModule : MentionUser", $"Command user: {Context.User.Username}, User pinged: {user.Username}"));
            await RespondAsync($"User to ping: <@{user.Id}>");
        }

        [MessageCommand("Author")]
        public async Task MessageAuthor(IMessage message)
        {
            await _logger.Log(new LogMessage(LogSeverity.Info, "CreateAccountModule: MessageAuthor", $"Command user: {Context.User.Username}, ChannelID/MessageID: {message.Channel.Id}/{message.Id}"));
            await RespondAsync($"Message author: <@{message.Author.Id}>");
        }
    }
}
