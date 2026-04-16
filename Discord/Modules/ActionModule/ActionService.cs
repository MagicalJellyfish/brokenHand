using System.Net.Http.Json;
using System.Web;
using brokenHand.Requests.Models;
using Discord;

namespace brokenHand.Discord.Modules.ActionModule
{
    public class ActionService
    {
        private IHttpClientFactory _httpClientFactory;

        public ActionService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<EmbedBuilder>> Ability(
            ulong discordId,
            string? charId,
            string? shortcut,
            string? targets,
            string? selfModifier,
            string? targetModifier,
            string? damageModifier
        )
        {
            string requestRoute = $"Actions/ability?discordId={discordId}";

            if (charId != null)
            {
                requestRoute += $"&charId={charId}";
            }

            if (shortcut != null)
            {
                requestRoute += $"&shortcut={HttpUtility.UrlEncode(shortcut)}";
            }

            if (targets != null)
            {
                requestRoute += $"&targets={HttpUtility.UrlEncode(targets)}";
            }

            if (selfModifier != null)
            {
                requestRoute += $"&selfModifier={HttpUtility.UrlEncode(selfModifier)}";
            }
            if (targetModifier != null)
            {
                requestRoute += $"&targetModifier={HttpUtility.UrlEncode(targetModifier)}";
            }
            if (damageModifier != null)
            {
                requestRoute += $"&damageModifier={HttpUtility.UrlEncode(damageModifier)}";
            }

            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .GetAsync(requestRoute);

            List<EmbedResponse> embeds =
                await response.Content.ReadFromJsonAsync<List<EmbedResponse>>() ?? [];
            return embeds.Select(Constants.EmbedsFromResponse).ToList();
        }
    }
}
