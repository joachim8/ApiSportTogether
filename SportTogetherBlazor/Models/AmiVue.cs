using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models
{
    public partial class AmiVue
    {
        [JsonPropertyName("AmiId")]
        public int AmiId { get; set; }
        [JsonPropertyName("UtilisateurId")]
        public int UtilisateurId { get; set; }
        [JsonPropertyName("Pseudo")]
        public string Pseudo { get; set; } = null!;

        [JsonPropertyName("UrlProfilImage")]
        public string UrlProfilImage { get; set; } = null!;
        [JsonPropertyName("DescriptionSport")]
        public string DescriptionSport { get; set; } = null!;
    }
}
