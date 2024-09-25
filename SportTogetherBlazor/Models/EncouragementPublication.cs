using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models;

public partial class EncouragementPublication
{
    [JsonPropertyName("EncouragementPublicationId")]
    public int EncouragementPublicationId { get; set; }
    [JsonPropertyName("UtilisateurId")]
    public int UtilisateurId { get; set; }
    [JsonPropertyName("PublicationsId")]
    public int PublicationId { get; set; }
    [JsonPropertyName("DateEncouragementPublication")]
    public DateTime? DateEncouragementPublication { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Publication")]
    public virtual Publication? Publication { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}
