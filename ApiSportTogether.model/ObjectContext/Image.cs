using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class Image
{
    [JsonPropertyName("ImagesId")]
    public int ImagesId { get; set; }

    [JsonPropertyName("UtilisateurId")]
    public int? UtilisateurId { get; set; }

    [JsonPropertyName("Url")]
    public string? Url { get; set; }

    [JsonPropertyName("Type")]
    public string? Type { get; set; }

    [JsonPropertyName("Timestamp")]
    public DateTime? Timestamp { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Annonces")]
    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
    [JsonIgnore]
    [JsonPropertyName("Publications")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();

    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateurs")]
    public virtual ICollection<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
}
