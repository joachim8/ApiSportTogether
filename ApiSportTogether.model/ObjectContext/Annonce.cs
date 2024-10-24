﻿using System.Text.Json.Serialization;
namespace ApiSportTogether.model.ObjectContext;
public partial class Annonce
{
    [JsonPropertyName("AnnoncesId")]
    public int AnnoncesId { get; set; }

    [JsonPropertyName("Auteur")]
    public int Auteur { get; set; }

    [JsonPropertyName("SportId")]
    public int? SportId { get; set; } 

    [JsonPropertyName("Titre")]
    public string? Titre { get; set; }

    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    [JsonPropertyName("GenreAttendu")]
    public string? GenreAttendu { get; set; }

    [JsonPropertyName("NombreParticipants")]
    public int? NombreParticipants { get; set; }

    [JsonPropertyName("Ville")]
    public string Ville { get; set; } = null!;

    [JsonPropertyName("Lieu")]
    public string Lieu { get; set; } = null!;
    [JsonPropertyName("Niveau")]
    public string Niveau { get; set; } = null!;
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
    [JsonIgnore]
    [JsonPropertyName("Participations")]
    public virtual ICollection<Participation> Participations { get; set; } = new List<Participation>();
    [JsonIgnore]
    [JsonPropertyName("NoteAnnonces")]
    public virtual ICollection<NoteAnnonce> NoteAnnonces { get; set; } = new List<NoteAnnonce>();
    [JsonIgnore]
    [JsonPropertyName("Sport")]
    public virtual Sport? Sport { get; set; }
}