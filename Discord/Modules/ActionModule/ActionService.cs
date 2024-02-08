using Discord;

namespace brokenHand.Discord.Modules.ActionModule
{
    public class ActionService
    {
        private HttpClient _httpClient;
        public ActionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> ActivateCombat(int charId, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync($"Character/activate?discordId={discordUserId}&charId={charId}", null);

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Character selected!",
                    Description = "Character " + await response.Content.ReadAsStringAsync() + " is now active!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }
    }
}