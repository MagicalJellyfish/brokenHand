using Discord;
using System.Net.Http.Json;
using System.Web;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicService
    {
        private HttpClient _httpClient;
        public BasicService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> RollAsync(string roll)
        {
            HttpResponseMessage response = await _httpClient.GetAsync("Actions/roll?rollString=" + HttpUtility.UrlEncode(roll));

            if (response.IsSuccessStatusCode)
            {
                RollResult result = await response.Content.ReadFromJsonAsync<RollResult>();

                var embed = new EmbedBuilder
                {
                    Title = result.Result.ToString(),
                    Description = Format.Sanitize(roll) + "\n= " + Format.Sanitize(result.Detail) + "\n= " + result.Result.ToString()
                };

                if (roll.StartsWith("1d20") || roll.StartsWith("d20"))
                {
                    if (result.Detail.StartsWith("[20]"))
                    {
                        embed.Color = Color.Green;
                    }
                    if (result.Detail.StartsWith("[1]"))
                    {
                        embed.Color = Color.Red;
                    }
                }

                return embed;
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }
    }
}
