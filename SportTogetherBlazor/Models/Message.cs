using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Message
{
    [JsonPropertyName("MessagesId")]
    public int MessagesId { get; set; }

    [JsonPropertyName("GroupeId")]
    public int? GroupeId { get; set; }

    [JsonPropertyName("UtilisateurId")]
    public int? UtilisateurId { get; set; }

    [JsonPropertyName("Contenu")]
    public string? Contenu { get; set; }
    [JsonPropertyName("NomUtilisateur")]
    public string? NomUtilisateur { get; set; }
    [JsonPropertyName("urlProfilImage")]
    public string? urlProfilImage { get; set; } = string.Empty;
    [JsonPropertyName("Timestamp")]
    public DateTime? Timestamp { get; set; }
    [JsonIgnore]
    [JsonPropertyName("VuMessages")]
    public virtual ICollection<VuMessage> VuMessages { get; set; } = new List<VuMessage>();
    [JsonIgnore]
    [JsonPropertyName("UtilisateurNavigation")]
    public virtual Utilisateur? UtilisateurNavigation { get; set; }
}

