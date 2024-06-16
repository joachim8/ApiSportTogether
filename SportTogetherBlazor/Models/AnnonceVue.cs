using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models;

    public class AnnonceVue
    {
        [JsonPropertyName("AnnoncesId")]
        public int AnnoncesId { get; set; }

        [JsonPropertyName("AuteurId")]
        public int AuteurId { get; set; }

        [JsonPropertyName("Auteur")]
        public string Auteur { get; set; } = null!;

        [JsonPropertyName("SportId")]
        public int? SportId { get; set; }

    [JsonPropertyName("SportName")]
    public string? SportName { get; set; }

    [JsonPropertyName("Titre")]
        public string? Titre { get; set; }

        [JsonPropertyName("Description")]
        public string? Description { get; set; }

        [JsonPropertyName("GenreAttendu")]
        public string? GenreAttendu { get; set; }

        [JsonPropertyName("NombreParticipants")]
        public int? NombreParticipants { get; set; }
    [JsonPropertyName("ListAnnonceImage")]
    public List<AnnonceImage> ListAnnonceImage { get; set; } = null!;

    [JsonPropertyName("Ville")]
        public string Ville { get; set; } = null!;

        [JsonPropertyName("Lieu")]
        public string Lieu { get; set; } = null!;
        [JsonPropertyName("DateHeureAnnonce")]
        public DateTime DateHeureAnnonce { get; set; }

    }

