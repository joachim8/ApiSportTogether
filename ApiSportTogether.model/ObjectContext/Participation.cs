using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
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
    [JsonIgnore]
    [JsonPropertyName("Annonce")]
    public virtual Annonce? Annonce { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Groupe")]
    public virtual Groupe? Groupe { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}