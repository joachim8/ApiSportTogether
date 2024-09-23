using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models
{

    public partial class PublicationAjoutVue
    {
        [JsonPropertyName("Publication")]
        public Publication Publication { get; set; }
        [JsonPropertyName("MediaUrls")]
        public List<string>? MediaUrls { get; set; }
    }
}
