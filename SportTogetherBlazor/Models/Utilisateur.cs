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

    [JsonIgnore]
    [JsonPropertyName("AmiUtilisateurId1Navigations")]
    public virtual ICollection<Ami> AmiUtilisateurId1Navigations { get; set; } = new List<Ami>();
    [JsonIgnore]
    [JsonPropertyName("AmiUtilisateurId2Navigations")]
    public virtual ICollection<Ami> AmiUtilisateurId2Navigations { get; set; } = new List<Ami>();
    [JsonIgnore]
    [JsonPropertyName("Annonces")]
    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();

    [JsonPropertyName("ProfileImages")]
    public virtual ICollection<ProfileImage> ProfileImages { get; set; } = new List<ProfileImage>();
    [JsonIgnore]
    [JsonPropertyName("Groupes")]
    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();
    [JsonIgnore]
    [JsonPropertyName("VuMessages")]
    public virtual ICollection<VuMessage> VuMessages { get; set; } = new List<VuMessage>();
    [JsonIgnore]
    [JsonPropertyName("MembreGroupes")]
    public virtual ICollection<MembreGroupe> MembreGroupes { get; set; } = new List<MembreGroupe>();
    [JsonIgnore]
    [JsonPropertyName("Publications")]
    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
    [JsonIgnore]
    [JsonPropertyName("SportFavoris")]
    public virtual ICollection<SportFavori> SportFavoris { get; set; } = new List<SportFavori>();
    [JsonIgnore]
    [JsonPropertyName("Messages")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    [JsonIgnore]
    [JsonPropertyName("PublicationCommentaires")]
    public virtual ICollection<PublicationCommentaire> PublicationCommentaires { get; set; } = new List<PublicationCommentaire>();

}
