using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class SportFavori
{
    [JsonPropertyName("SportFavoriId")]
    public int SportFavoriId { get; set; }

    [JsonPropertyName("SportsId")]
    public int? SportsId { get; set; }

    [JsonPropertyName("UtilisateursId")]
    public int? UtilisateursId { get; set; }

    [JsonPropertyName("Sports")]
    [JsonIgnore]
    public virtual Sport? Sports { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateurs")]
    public virtual Utilisateur? Utilisateurs { get; set; }
}
