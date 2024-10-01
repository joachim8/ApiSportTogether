using System.Text.Json.Serialization;


namespace SportTogetherBlazor.Models
{
    public class PublicationVue
    {
        [JsonPropertyName("PublicationsId")]
        public int PublicationsId { get; set; }
        [JsonPropertyName("UtilisateurId")]
        public int UtilisateurId { get; set; }
        [JsonPropertyName("Contenu")]
        public string? Contenu { get; set; }
        [JsonPropertyName("DatePublication")]
        public DateTime DatePublication { get; set; }
        [JsonPropertyName("PseudoUtilisateur")]
        public string? PseudoUtilisateur { get; set; } // Pour afficher le pseudo de l'utilisateur
        [JsonPropertyName("ImageUtilisateurUrl")]
        public string? ImageUtilisateurUrl { get; set; } // URL de l'image de profil de l'utilisateur
        [JsonPropertyName("MediaUrls")]
        public List<string>? MediaUrls { get; set; } // Liste des URL des images/vidéos associées à la publication
        [JsonPropertyName("NombreEncouragements")]
        public int NombreEncouragements { get; set; } // Nombre d'encouragements (emoji gros bras)
        [JsonPropertyName("Commentaires")]
        public List<CommentaireVue>? Commentaires { get; set; } // Liste des commentaires
        [JsonPropertyName("tempsDiff")]
        public string? tempsDiff { get; set; } // Temps diff
        [JsonPropertyName("IsEncourager")]
        public bool IsEncourager { get; set; }
        [JsonPropertyName("SportTag")]
        public string? SportTag { get; set; }
        [JsonPropertyName("Visibilite")]
        public bool Visibilite { get; set; }
    }
}
