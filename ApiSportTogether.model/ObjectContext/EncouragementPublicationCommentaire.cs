using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectContext;

public partial class EncouragementPublicationCommentaire
{
    [JsonPropertyName("PublicationsId")]
    public int EncouragementPublicationCommentaireId { get; set; }
    [JsonPropertyName("PublicationCommentaireId")]
    public int PublicationCommentaireId { get; set; }
    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }
    [JsonPropertyName("DateEncouragementPublicationCommentaire")]
    public DateTime? DateEncouragementPublicationCommentaire { get; set; }
    [JsonIgnore]
    [JsonPropertyName("PublicationCommentaire")]
    public virtual PublicationCommentaire? PublicationCommentaire { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}
