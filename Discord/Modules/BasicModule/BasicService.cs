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

                return Constants.RollResultEmbed(roll, result);
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

                return Constants.RollResultEmbed(roll, result);
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }
    }
}
