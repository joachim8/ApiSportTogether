using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Participation
{
    [JsonPropertyName("ParticipationsId")]
    public int ParticipationsId { get; set; }

    [JsonPropertyName("UtilisateurId")]
    public int? UtilisateurId { get; set; }

    [JsonPropertyName("AnnonceId")]
    public int? AnnonceId { get; set; }

    [JsonPropertyName("GroupeId")]
    public int? GroupeId { get; set; }

    [JsonPropertyName("DateParticipation")]
    public DateTime? DateParticipation { get; set; }
}