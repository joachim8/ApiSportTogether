using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Annonce
{
    public int AnnoncesId { get; set; }

    public int Auteur { get; set; }

    public int? SportId { get; set; }

    public string? Titre { get; set; }

    public string? Description { get; set; }

    public string? GenreAttendu { get; set; }

    public int? NombreParticipants { get; set; }

    public int? ImageId { get; set; }

    public string Ville { get; set; } = null!;

    public string Lieu { get; set; } = null!;

    public virtual Utilisateur AuteurNavigation { get; set; } = null!;

    public virtual ICollection<Groupe> Groupes { get; set; } = new List<Groupe>();

    public virtual Image? Image { get; set; }

    public virtual Sport? Sport { get; set; }
}
