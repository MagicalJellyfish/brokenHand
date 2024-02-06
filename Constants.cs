using Discord;

namespace brokenHand
{
    public class Constants
    {
        public static async Task<EmbedBuilder> ErrorEmbedFromResponseAsync(HttpResponseMessage response)
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
                    Description = response.ReasonPhrase,
                    Color = Color.Red
                };
            }
            
        }
    }
}
