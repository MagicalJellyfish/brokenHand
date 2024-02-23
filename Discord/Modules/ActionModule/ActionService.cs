using Discord;
using System;
using System.Text.Json;
using System.Web;

namespace brokenHand.Discord.Modules.ActionModule
{
    public class ActionService
    {
        private HttpClient _httpClient;
        public ActionService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> ActivateChar(int charId, ulong discordUserId)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync($"Characters/activate?discordId={discordUserId}&charId={charId}", null);

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

        public async Task<List<EmbedBuilder>> CharAbility(string charId, string shortcut, string? targets, bool targetShortcuts = true)
        {
            string requestRoute = $"Actions/ability/{charId}/{HttpUtility.UrlEncode(shortcut)}";
            if(targets != null)
            {
                requestRoute += $"?targets={HttpUtility.UrlEncode(targets)}&targetShortcuts={targetShortcuts}";
            }
            HttpResponseMessage response = await _httpClient.GetAsync(requestRoute);

            return await AbilityResponseAsync(response);
        }

        public async Task<List<EmbedBuilder>> Ability(ulong discordId, string shortcut, string? targets, bool targetShortcuts = true)
        {
            string requestRoute = $"Actions/activeAbility/{discordId}/{HttpUtility.UrlEncode(shortcut)}";
            if (targets != null)
            {
                requestRoute += $"?targets={HttpUtility.UrlEncode(targets)}&targetShortcuts={targetShortcuts}";
            }
            HttpResponseMessage response = await _httpClient.GetAsync(requestRoute);

            return await AbilityResponseAsync(response);
        }

        private async Task<List<EmbedBuilder>> AbilityResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                List<EmbedBuilder> embeds = new List<EmbedBuilder>();
                JsonElement resObj = JsonDocument.Parse(response.Content.ReadAsStream()).RootElement;

                foreach (JsonElement message in resObj.EnumerateArray())
                {
                    EmbedBuilder embed = new EmbedBuilder()
                    {
                        Title = message.GetProperty("title").ToString(),
                        Description = message.GetProperty("description").ToString()
                    };

                    if(message.TryGetProperty("color", out var color))
                    {
                        switch(color.ToString())
                        {
                            case "Green":
                                embed.Color = Color.Green;
                                break;
                            case "Red":
                                embed.Color = Color.Red;
                                break;
                        }
                    }
                    embeds.Add(embed);
                }

                return embeds;
            }
            else
            {
                return [await Constants.ErrorEmbedFromResponseAsync(response)];
            }
        }
    }
}