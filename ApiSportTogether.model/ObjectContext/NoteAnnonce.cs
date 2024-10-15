using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectContext;

public partial class NoteAnnonce
{
    [JsonPropertyName("NoteAnnonceId")]
    public int NoteAnnonceId { get; set; }
    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }
    [JsonPropertyName("AnnoncesId")]
    public int AnnonceId { get; set; }
    [JsonPropertyName("Commentaire")]
    public string Commentaire { get; set; } = null!;
    [JsonPropertyName("Note")]
    public decimal Note { get; set; }
    [JsonPropertyName("IsPublic")]
    public bool IsPublic { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Annonce")]
    public virtual Annonce? Annonce { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}
