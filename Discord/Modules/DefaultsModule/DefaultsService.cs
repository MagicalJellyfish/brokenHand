using System.Web;
using Discord;

namespace brokenHand.Discord.Modules.DefaultsModule
{
    public class DefaultsService
    {
        private HttpClient _httpClient;

        public DefaultsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> DefaultChar(int charId, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync(
                $"Defaults/character?discordId={discordUserId}&charId={charId}",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Character selected!",
                    Description =
                        "Character "
                        + await response.Content.ReadAsStringAsync()
                        + " is now active!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> DefaultAbility(string shortcut, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync(
                $"Defaults/ability?discordId={discordUserId}&shortcut={HttpUtility.UrlEncode(shortcut)}",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Default Ability set!",
                    Description = $"Your default ability is now {shortcut}!"
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> DefaultTargets(string targets, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync(
                $"Defaults/targets?discordId={discordUserId}&targets={HttpUtility.UrlEncode(targets)}",
                null
            );

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Default Target(s) set!",
                    Description = $"Your default target parameter is now {targets}!"
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }
    }
}
