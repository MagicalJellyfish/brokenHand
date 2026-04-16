using System.Web;
using Discord;

namespace brokenHand.Discord.Modules.DefaultsModule
{
    public class DefaultsService
    {
        private IHttpClientFactory _httpClientFactory;

        public DefaultsService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EmbedBuilder> DefaultChar(int charId, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PatchAsync(
                $"Defaults/character?discordId={discordUserId}&charId={charId}",
                null
            );

            return new EmbedBuilder
            {
                Title = "Character selected!",
                Description =
                    $"Character {await response.Content.ReadAsStringAsync()} is now active!",
            };
        }

        public async Task<EmbedBuilder> DefaultAbility(string shortcut, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PatchAsync(
                $"Defaults/ability?discordId={discordUserId}&shortcut={HttpUtility.UrlEncode(shortcut)}",
                null
            );

            return new EmbedBuilder
            {
                Title = "Default Ability set!",
                Description = $"Your default ability is now {shortcut}!",
            };
        }

        public async Task<EmbedBuilder> DefaultTargets(string targets, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PatchAsync(
                $"Defaults/targets?discordId={discordUserId}&targets={HttpUtility.UrlEncode(targets)}",
                null
            );

            return new EmbedBuilder
            {
                Title = "Default Target(s) set!",
                Description = $"Your default target parameter is now {targets}!",
            };
        }
    }
}
