﻿using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class Groupe
{
    [JsonPropertyName("GroupesId")]
    public int GroupesId { get; set; }

    [JsonPropertyName("AnnonceId")]
    public int? AnnonceId { get; set; }

    [JsonPropertyName("DateCreation")]
    public DateTime? DateCreation { get; set; }

    [JsonPropertyName("DateSuppression")]
    public DateTime? DateSuppression { get; set; }

    [JsonPropertyName("Annonce")]
    public virtual Annonce? Annonce { get; set; }
    [JsonPropertyName("ChefDuGroupe")]
    public int ChefDuGroupe { get; set; }
    [JsonPropertyName("Nom")]
    public string Nom { get; set; } = string.Empty;
    [JsonPropertyName("LastMessage")]
    public string? LastMessage { get; set; }
    [JsonPropertyName("ChefDuGroupeNavigation")]
    public virtual Utilisateur ChefDuGroupeNavigation { get; set; } = null!;
    [JsonIgnore]
    [JsonPropertyName("ChefDuGroupeNavigation")]
    public virtual ICollection<MembreGroupe> MembreGroupes { get; set; } = new List<MembreGroupe>();
    public int? nbreMessageVu { get; set; }
}

