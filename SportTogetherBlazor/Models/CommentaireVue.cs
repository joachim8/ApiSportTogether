using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models
{
    public class CommentaireVue
    {
        [JsonPropertyName("CommentaireId")]
        public int CommentaireId { get; set; }
        [JsonPropertyName("UtilisateurId")]
        public int UtilisateurId { get; set; }
        [JsonPropertyName("PseudoUtilisateur")]
        public string? PseudoUtilisateur { get; set; } // Pseudo de l'utilisateur ayant posté le commentaire
        [JsonPropertyName("Contenu")]
        public string Contenu { get; set; }
        [JsonPropertyName("DateCommentaire")]
        public DateTime DateCommentaire { get; set; }
        [JsonPropertyName("NombreEncouragementCommentaire")]
        public int NombreEncouragementCommentaire { get; set; }
        [JsonPropertyName("ImageUtilisateurUrl")]
        public string? ImageUtilisateurUrl { get; set; } // URL de l'image de profil de l'utilisateur
    }
}
