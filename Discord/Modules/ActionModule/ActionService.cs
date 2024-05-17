using System.Text.Json;
using System.Web;
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

        public async Task<List<EmbedBuilder>> Ability(
            ulong discordId,
            string? charId,
            string? shortcut,
            string? targets
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
            HttpResponseMessage response = await _httpClient.GetAsync(requestRoute);

            return await AbilityResponseAsync(response);
        }

        private async Task<List<EmbedBuilder>> AbilityResponseAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                List<EmbedBuilder> embeds = new List<EmbedBuilder>();
                JsonElement resObj = JsonDocument
                    .Parse(response.Content.ReadAsStream())
                    .RootElement;

                foreach (JsonElement message in resObj.EnumerateArray())
                {
                    EmbedBuilder embed = new EmbedBuilder()
                    {
                        Title = message.GetProperty("title").ToString(),
                        Description = message.GetProperty("description").ToString()
                    };

                    if (message.TryGetProperty("color", out var color))
                    {
                        switch (color.ToString())
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
