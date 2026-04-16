using brokenHand.Requests.Models;
using Discord;

namespace brokenHand
{
    public class Constants
    {
        public const string BrokenHeartClient = nameof(BrokenHeartClient);

        public static EmbedBuilder EmbedsFromResponse(EmbedResponse message)
        {
            EmbedBuilder embed = new EmbedBuilder()
            {
                Title = Format.Sanitize(message.Title),
                Description = Format.Sanitize(message.Description),
            };

            if (message.Color != null)
            {
                switch (message.Color)
                {
                    case "Green":
                        embed.Color = Color.Green;
                        break;
                    case "Red":
                        embed.Color = Color.Red;
                        break;
                }
            }
            return embed;
        }

        public static EmbedBuilder RollResultEmbed(string roll, RollResult result)
        {
            var embed = new EmbedBuilder
            {
                Title = Format.Sanitize(result.Result.ToString()),
                Description = Format.Sanitize(result.Detail),
            };

            if (result.CriticalSuccess)
            {
                embed.Color = Color.Green;
            }
            if (result.CriticalFailure)
            {
                embed.Color = Color.Red;
            }

            return embed;
        }
    }
}
