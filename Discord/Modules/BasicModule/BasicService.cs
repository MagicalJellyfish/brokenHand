using System.Net.Http.Json;
using System.Web;
using brokenHand.Requests.Models;
using Discord;

namespace brokenHand.Discord.Modules.Basic
{
    public class BasicService
    {
        private IHttpClientFactory _httpClientFactory;

        public BasicService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
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

            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .GetAsync(url);

            List<RollResult> results =
                await response.Content.ReadFromJsonAsync<List<RollResult>?>() ?? [];

            return results.Select(x => Constants.RollResultEmbed(roll, x)).ToList();
        }
    }
}
