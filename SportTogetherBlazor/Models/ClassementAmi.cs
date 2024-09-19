using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models
{
    public class ClassementAmi
    {
        [JsonPropertyName("Pseudo")]
        public string Pseudo { get; set; } = null!;

        [JsonPropertyName("UrlProfilImage")]
        public string? UrlProfilImage { get; set; }

        [JsonPropertyName("Classement")]
        public int Classement { get; set; }
        [JsonPropertyName("NombreActivites")]
        public int NombreActivites { get; set; }
    }
}
