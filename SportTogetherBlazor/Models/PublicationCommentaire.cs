using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models
{
    public class PublicationCommentaire
    {
        [JsonPropertyName("CommentaireId")]
        public int CommentaireId { get; set; }
        [JsonPropertyName("Contenu")]
        public string Contenu { get; set; } = null!;
        [JsonPropertyName("DateCommentaire")]
        public DateTime DateCommentaire { get; set; }
        [JsonPropertyName("PublicationId")]
        public int PublicationId { get; set; }
        [JsonPropertyName("UtilisateurId")]
        public int UtilisateurId { get; set; }
        [JsonIgnore]
        [JsonPropertyName("Publication")]
        public virtual Publication? Publication { get; set; } = null!;
        [JsonIgnore]
        [JsonPropertyName("Utilisateur")]
        public virtual Utilisateur? Utilisateur { get; set; } = null!;
        [JsonIgnore]
        [JsonPropertyName("EncouragementPublicationCommentaires")]
        public virtual ICollection<EncouragementPublicationCommentaire> EncouragementPublicationCommentaires { get; set; } = new List<EncouragementPublicationCommentaire>();
    }
}
