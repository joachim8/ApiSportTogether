using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class Sport
{
    [JsonPropertyName("SportsId")]
    public int SportsId { get; set; }

    [JsonPropertyName("Nom")]
    public string? Nom { get; set; }

    [JsonPropertyName("Annonces")]
    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();

    [JsonPropertyName("SportFavoris")]
    public virtual ICollection<SportFavori> SportFavoris { get; set; } = new List<SportFavori>();
}
