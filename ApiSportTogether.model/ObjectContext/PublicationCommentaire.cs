using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectContext;

public partial class PublicationCommentaire
{
    [JsonPropertyName("CommentaireId")]
    public int CommentaireId { get; set; }
    [JsonPropertyName("Contenu")]
    public string Contenu { get; set; } = null!;
    [JsonPropertyName("DateCommentaire")]
    public DateTime DateCommentaire { get; set; }
    [JsonPropertyName("PublicationId")]
    public int PublicationId { get; set; }
    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }
    [JsonPropertyName("NombreEncouragementCommentaire")]
    public int NombreEncouragementCommentaire { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Publication")]
    public virtual Publication? Publication { get; set; } = null!;
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; } = null!;
}
