using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectContext;

public partial class VuMessage
{
    [JsonPropertyName("IdMessageVu")]
    public int IdMessageVu { get; set; }
    [JsonPropertyName("Vu")]
    public bool Vu { get; set; }
    [JsonPropertyName("UtilisateurId")]
    public int? UtilisateurId { get; set; }
    [JsonPropertyName("messages_id")]
    public int? messages_id { get; set; }
    [JsonIgnore]
    [JsonPropertyName("IdMessageNavigation")]
    public virtual Message? IdMessageNavigation { get; set; }
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}
