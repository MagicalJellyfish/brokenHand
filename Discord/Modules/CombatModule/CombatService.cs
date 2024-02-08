using Discord;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace brokenHand.Discord.Modules.CombatModule
{
    public class CombatService
    {
        private HttpClient _httpClient;
        public CombatService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<EmbedBuilder> StartCombat()
        {
            HttpResponseMessage response = await _httpClient.PostAsync("Combat", null);

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Combat created!",
                    Description = "Id is " + await response.Content.ReadAsStringAsync(),
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> EndCombat()
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync("Combat");

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Combat ended!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> ActivateCombat(int id)
        {
            HttpResponseMessage response = await _httpClient.PatchAsync("Combat/activate/" + id, null);

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Combat activated!",
                    Description = "Combat " + id + " is now active!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> AddParticipant(int id, int? initRoll, string? shortcut)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"Combat/add-participant?id={id}&initRoll={initRoll}&shortcut={shortcut}", null);

            if (response.IsSuccessStatusCode)
            {
                JsonElement resObj = JsonDocument.Parse(response.Content.ReadAsStream()).RootElement;
                return new EmbedBuilder
                {
                    Title = "Character added!",
                    Description = "Character " + resObj.GetProperty("name").ToString() + " (short " + resObj.GetProperty("shortcut").ToString() +  ") is now in combat with initiative " + resObj.GetProperty("initRoll").ToString() + "!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> RemoveParticipant(string shortcut)
        {
            HttpResponseMessage response = await _httpClient.DeleteAsync($"Combat/remove-participant?shortcut={shortcut}");

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Character removed!",
                    Description = "Character " + shortcut + " removed from combat!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<EmbedBuilder> AddEvent(string name, int round, int init, bool secret)
        {
            HttpResponseMessage response = await _httpClient.PostAsync($"Combat/add-event", new StringContent(JsonSerializer.Serialize(new { name, round, secret, init }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                return new EmbedBuilder
                {
                    Title = "Event added!",
                    Description = "Event \"" + name + "\" is now primed!",
                };
            }
            else
            {
                return await Constants.ErrorEmbedFromResponseAsync(response);
            }
        }

        public async Task<List<EmbedBuilder>> NextTurn()
        {
            List<EmbedBuilder> embeds = new List<EmbedBuilder>();
            HttpResponseMessage response = await _httpClient.PatchAsync($"Combat/nextTurn", null);

            if (response.IsSuccessStatusCode)
            {
                JsonElement resObj = JsonDocument.Parse(response.Content.ReadAsStream()).RootElement;

                foreach(JsonElement message in resObj.EnumerateArray())
                {
                    embeds.Add(new EmbedBuilder()
                    {
                        Title = message.GetProperty("title").ToString(),
                        Description = message.GetProperty("description").ToString(),
                    });
                }

                return embeds;
            }
            else
            {
                embeds.Add(await Constants.ErrorEmbedFromResponseAsync(response));
                return embeds;
            }
        }
    }
}
