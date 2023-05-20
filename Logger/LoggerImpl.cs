using Discord;

namespace DiscordBSApp.Logger
{
    public class LoggerImpl : Logger
    {
        public override async Task Log(LogMessage message)
        {
            await Task.Run(() => LogToConsoleAsync(this, message));
        }
        private void LogToConsoleAsync<T>(T logger, LogMessage message) where T : ILogger
        {
            Console.WriteLine($"guid:{_guid} :" + message);
        }
    }
}
