using System.Net.Http.Json;
using System.Web;
using Discord;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicService
    {
        private HttpClient _httpClient;

        public BasicService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> CharRoll(string roll, ulong discordId, int? charId)
        {
            HttpResponseMessage response;
            if (charId == null)
            {
                response = await _httpClient.GetAsync(
                    $"Actions/rollActiveChar/{discordId}?rollString={HttpUtility.UrlEncode(roll)}"
                );
            }
            else
            {
                response = await _httpClient.GetAsync(
                    $"Actions/rollChar/{charId}?rollString={HttpUtility.UrlEncode(roll)}"
                );
            }

            if (response.IsSuccessStatusCode)
            {
                RollResult result = await response.Content.ReadFromJsonAsync<RollResult>();

                return RollResultEmbed(roll, result);
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> Roll(string roll)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(
                $"Actions/roll?rollString={HttpUtility.UrlEncode(roll)}"
            );

            if (response.IsSuccessStatusCode)
            {
                RollResult result = await response.Content.ReadFromJsonAsync<RollResult>();

                return RollResultEmbed(roll, result);
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        private EmbedBuilder RollResultEmbed(string roll, RollResult result)
        {
            var embed = new EmbedBuilder
            {
                Title = result.Result.ToString(),
                Description = Format.Sanitize(result.Detail)
            };

            if (roll.StartsWith("1d20") || roll.StartsWith("d20"))
            {
                string resultDetail = result.Detail.Split("=")[1][1..];
                if (resultDetail.StartsWith("[20]"))
                {
                    embed.Color = Color.Green;
                }
                if (resultDetail.StartsWith("[1]"))
                {
                    embed.Color = Color.Red;
                }
            }

            return embed;
        }
    }
}
