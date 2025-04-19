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

        public async Task<List<EmbedBuilder>> Roll(
            string roll,
            ulong discordId,
            int? charId,
            int repeat
        )
        {
            string url =
                "Actions/"
                + (charId == null ? $"rollActiveChar/{discordId}" : $"rollChar/{charId}")
                + $"?rollString={HttpUtility.UrlEncode(roll)}&repeat={HttpUtility.UrlEncode(repeat.ToString())}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                List<RollResult> result = await response.Content.ReadFromJsonAsync<
                    List<RollResult>
                >();

                return Constants.RollResultEmbed(roll, result);
            }
            else
            {
                return [await Constants.ErrorEmbedFromResponseAsync(response)];
            }
        }
    }
}
