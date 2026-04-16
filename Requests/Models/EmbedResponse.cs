namespace brokenHand.Requests.Models
{
    public class EmbedResponse
    {
        public EmbedResponse(string title, string? description = null, string? color = null)
        {
            Title = title;
            Description = description;
            Color = color;
        }

        public string Title { get; set; }
        public string? Description { get; set; }
        public string? Color { get; set; }
    }
}
