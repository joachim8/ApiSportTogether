using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class Publication
{
    [JsonPropertyName("PublicationsId")]
    public int PublicationsId { get; set; }

    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }

    [JsonPropertyName("Contenu")]
    public string? Contenu { get; set; }

    [JsonPropertyName("ImageId")]
    public int ImageId { get; set; }

    [JsonPropertyName("Image")]
    public virtual Image Image { get; set; } = null!;

    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur Utilisateur { get; set; } = null!;
}
