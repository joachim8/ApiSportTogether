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
    [JsonIgnore]
    [JsonPropertyName("PublicationImages")]
    public virtual ICollection<PublicationImage> PublicationImages { get; set; } = new List<PublicationImage>();

    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur Utilisateur { get; set; } = null!;
}
