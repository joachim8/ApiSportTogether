using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Ami
{
    [JsonPropertyName("AmisId")]
    public int AmisId { get; set; }

    [JsonPropertyName("UtilisateurId1")]
    public int? UtilisateurId1 { get; set; }

    [JsonPropertyName("UtilisateurId2")]
    public int? UtilisateurId2 { get; set; }

    [JsonPropertyName("DateAjout")]
    public DateTime? DateAjout { get; set; }

    [JsonPropertyName("UtilisateurId1Navigation")]
    public virtual Utilisateur? UtilisateurId1Navigation { get; set; }

    [JsonPropertyName("UtilisateurId2Navigation")]
    public virtual Utilisateur? UtilisateurId2Navigation { get; set; }
}