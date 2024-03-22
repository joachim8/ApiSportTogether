using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class Groupe
{
    [JsonPropertyName("GroupesId")]
    public int GroupesId { get; set; }

    [JsonPropertyName("AnnonceId")]
    public int? AnnonceId { get; set; }

    [JsonPropertyName("DateCreation")]
    public DateTime? DateCreation { get; set; }

    [JsonPropertyName("DateSuppression")]
    public DateTime? DateSuppression { get; set; }

    [JsonPropertyName("Annonce")]
    public virtual Annonce? Annonce { get; set; }
}

