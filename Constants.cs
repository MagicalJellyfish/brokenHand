using brokenHand.Discord.Modules.Basic;
using Discord;

namespace brokenHand
{
    public class Constants
    {
        public static async Task<EmbedBuilder> ErrorEmbedFromResponseAsync(
            HttpResponseMessage response
        )
        {
            string message = await response.Content.ReadAsStringAsync();
            if (!message.StartsWith('{'))
            {
                return new EmbedBuilder
                {
                    Title = "Error!",
                    Description = message,
                    Color = Color.Red
                };
            }
            else
            {
                return new EmbedBuilder
                {
                    Title = "Error!",
                    Description = response.StatusCode + ": " + response.ReasonPhrase,
                    Color = Color.Red
                };
            }
        }

        public static EmbedBuilder RollResultEmbed(string roll, RollResult result)
        {
            var embed = new EmbedBuilder
            {
                Title = result.Result.ToString(),
                Description = Format.Sanitize(result.Detail)
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
