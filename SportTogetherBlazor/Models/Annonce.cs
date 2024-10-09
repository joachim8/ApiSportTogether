using SportTogetherBlazor.Regles;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Annonce
{
    [JsonPropertyName("AnnoncesId")]
    public int AnnoncesId { get; set; }

    [JsonPropertyName("Auteur")]
    public int Auteur { get; set; }

    [JsonPropertyName("SportId")]
    public int? SportId { get; set; }

    [JsonPropertyName("Titre")]
    [Required(ErrorMessage = "Le titre est requis.")]
    [StringLength(100, ErrorMessage = "Le titre ne peut pas dépasser 100 caractères.")]
    public string? Titre { get; set; }


    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Le genre est requis.")]
    [JsonPropertyName("GenreAttendu")]
    public string? GenreAttendu { get; set; }

    [JsonPropertyName("NombreParticipants")]
    [Range(1, int.MaxValue, ErrorMessage = "Le nombre de participants doit être supérieur à 0.")]

    public int? NombreParticipants { get; set; }
    [Required(ErrorMessage = "La ville est requise.")]
    [JsonPropertyName("Ville")]
    public string Ville { get; set; } = null!;
    [Required(ErrorMessage = "Le lieu est requis.")]
    [JsonPropertyName("Lieu")]
    public string Lieu { get; set; } = null!;
    [JsonPropertyName("Niveau")]
    public string Niveau { get; set; } = null!;

    [Required(ErrorMessage = "La date et l'heure de l'annonce sont requises.")]
    [FutureOrTodayDate(ErrorMessage = "La date doit être aujourd'hui ou dans le futur.")]
    [JsonPropertyName("DateHeureAnnonce")]
    public DateTime DateHeureAnnonce { get; set; }
    [JsonPropertyName("NoteAnnonce")]
    public decimal? NoteAnnonce { get; set; }
    [JsonIgnore]
    [JsonPropertyName("AuteurNavigation")]
    public virtual Utilisateur? AuteurNavigation { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Groupes")]
    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();
 


    [JsonPropertyName("Sport")]
    public virtual Sport? Sport { get; set; }
}