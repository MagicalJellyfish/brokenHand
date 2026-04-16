using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Web;
using brokenHand.Requests.Models;
using Discord;

namespace brokenHand.Discord.Modules.CombatModule
{
    public class CombatService
    {
        private IHttpClientFactory _httpClientFactory;

        public CombatService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<EmbedBuilder> StartCombat()
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PostAsync("Combat", null);

            return new EmbedBuilder
            {
                Title = "Combat created!",
                Description = $"Id is {await response.Content.ReadAsStringAsync()}",
            };
        }

        public async Task<EmbedBuilder> EndCombat()
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .DeleteAsync("Combat");

            return new EmbedBuilder { Title = "Combat ended!" };
        }

        public async Task<EmbedBuilder> ActivateCombat(int id)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PatchAsync(
                $"Combat/activate/{id}",
                null
            );

            return new EmbedBuilder
            {
                Title = "Combat activated!",
                Description = $"Combat {id} is now active!",
            };
        }

        public async Task<EmbedBuilder> AddParticipant(int id, int? initRoll, string? shortcut)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PostAsync(
                $"Combat/add-participant?id={id}&initRoll={initRoll}&shortcut={HttpUtility.UrlEncode(shortcut)}",
                null
            );

            AddParticipantEmbedResponse embed =
                await response.Content.ReadFromJsonAsync<AddParticipantEmbedResponse>()
                ?? throw new Exception("Failed to parse response from add-participant");

            return new EmbedBuilder
            {
                Title = "Character added!",
                Description =
                    $"Character {embed.Name} (short \"{embed.Shortcut}\") is now in combat with initiative {embed.InitRoll}!",
            };
        }

        public async Task<EmbedBuilder> RemoveParticipant(string shortcut)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .DeleteAsync(
                $"Combat/remove-participant?shortcut={HttpUtility.UrlEncode(shortcut)}"
            );

            return new EmbedBuilder
            {
                Title = "Character removed!",
                Description = $"Character {shortcut} removed from combat!",
            };
        }

        public async Task<EmbedBuilder> AddEvent(string name, int round, int init, bool secret)
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PostAsync(
                "Combat/add-event",
                new StringContent(
                    JsonSerializer.Serialize(
                        new
                        {
                            name,
                            round,
                            secret,
                            init,
                        }
                    ),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            return new EmbedBuilder
            {
                Title = "Event added!",
                Description = $"Event \"{name}\" is now primed!",
            };
        }

        public async Task<List<EmbedBuilder>> NextTurn()
        {
            HttpResponseMessage response = await _httpClientFactory
                .CreateClient(Constants.BrokenHeartClient)
                .PatchAsync("Combat/next-turn", null);

            List<EmbedResponse> embeds =
                await response.Content.ReadFromJsonAsync<List<EmbedResponse>>() ?? [];
            return embeds.Select(Constants.EmbedsFromResponse).ToList();
        }
    }
}
