using ApiSportTogether.model.ObjectContext;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectVue
{
    public partial class PublicationAjoutVue
    {
        [JsonPropertyName("Publication")]
        public required Publication Publication { get; set; }
        [JsonPropertyName("MediaUrls")]
        public List<string>? MediaUrls { get; set; }
    }
}
