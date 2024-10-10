﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ApiSportTogether.model.ObjectContext;

public partial class NotificationUtilisateur
{
    [JsonPropertyName("NotificationId")]
    public int NotificationId { get; set; }
    [JsonPropertyName("UtilisateursId")]
    public int UtilisateurId { get; set; }
    [JsonPropertyName("TypeNotification")]
    public string TypeNotification { get; set; } = null!;
    [JsonPropertyName("DateNotification")]
    public DateTime DateNotification { get; set; }
    [JsonPropertyName("Vu")]
    public bool Vu { get; set; }
    [JsonPropertyName("Contenu")]
    public string Contenu { get; set; } = null!;
    [JsonIgnore]
    [JsonPropertyName("Utilisateur")]
    public virtual Utilisateur? Utilisateur { get; set; }
}