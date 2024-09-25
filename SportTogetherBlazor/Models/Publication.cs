using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Publication
{
    [JsonPropertyName("PublicationsId")]
    public int PublicationsId { get; set; }

    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }

    [JsonPropertyName("Contenu")]
    public string? Contenu { get; set; }
    [JsonPropertyName("DatePublication")]
    public DateTime DatePublication { get; set; }
    [JsonPropertyName("NombreEncouragement")]
    public int NombreEncouragement { get; set; }
    [JsonIgnore]
    [JsonPropertyName("PublicationImages")]
    public virtual ICollection<PublicationImage> PublicationImages { get; set; } = new List<PublicationImage>();
    [JsonIgnore]
    [JsonPropertyName("PublicationCommentaires")]
    public virtual ICollection<PublicationCommentaire> PublicationCommentaires { get; set; } = new List<PublicationCommentaire>();
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
    [JsonIgnore]
    [JsonPropertyName("EncouragementPublications")]
    public virtual ICollection<EncouragementPublication> EncouragementPublications { get; set; } = new List<EncouragementPublication>();
    [JsonPropertyName("SportTag")]
    public string? SportTag { get; set; }
}
