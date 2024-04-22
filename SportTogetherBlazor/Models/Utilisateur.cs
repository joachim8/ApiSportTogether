using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Utilisateur
{
    [JsonPropertyName("UtilisateursId")]
    public int UtilisateursId { get; set; }
    [JsonPropertyName("Nom")]
    public string Nom { get; set; } = null!;
    [JsonPropertyName("Prenom")]
    public string Prenom { get; set; } = null!;
    [JsonPropertyName("Pseudo")]
    public string? Pseudo { get; set; }
    [JsonPropertyName("MotDePasse")]
    public string MotDePasse { get; set; } = null!;
    [JsonPropertyName("Genre")]
    public string? Genre { get; set; }
    [JsonPropertyName("Age")]
    public int? Age { get; set; }
    [JsonPropertyName("Ville")]
    public string? Ville { get; set; }
    [JsonPropertyName("Email")]
    public string? Email { get; set; }
    [JsonPropertyName("Description")]
    public string? Description { get; set; }
    [JsonPropertyName("Etat")]
    public string? Etat { get; set; }
    [JsonPropertyName("EnLigne")]
    public bool? EnLigne { get; set; }
    [JsonPropertyName("ImageId")]
    public int? ImageId { get; set; }
    [JsonIgnore]
    [JsonPropertyName("AmiUtilisateurId1Navigations")]
    public virtual ICollection<Ami> AmiUtilisateurId1Navigations { get; set; } = new List<Ami>();
    [JsonIgnore]
    [JsonPropertyName("AmiUtilisateurId2Navigations")]
    public virtual ICollection<Ami> AmiUtilisateurId2Navigations { get; set; } = new List<Ami>();
    [JsonIgnore]
    [JsonPropertyName("Annonces")]
    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
    [JsonPropertyName("Image")]
    public virtual Image? Image { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Images")]
    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
    [JsonIgnore]
    [JsonPropertyName("Publications")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
    [JsonIgnore]
    [JsonPropertyName("SportFavoris")]
    public virtual ICollection<SportFavori> SportFavoris { get; set; } = new List<SportFavori>();
}
