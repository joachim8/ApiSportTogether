using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Utilisateur
{
    public int UserId { get; set; }

    public string Nom { get; set; } = null!;

    public string Prenom { get; set; } = null!;

    public string? Pseudo { get; set; }

    public string MotDePasse { get; set; } = null!;

    public string? Genre { get; set; }

    public int? Age { get; set; }

    public string? Ville { get; set; }

    public string? SportsFavoris { get; set; }

    public string? Email { get; set; }

    public string? Description { get; set; }

    public string? Etat { get; set; }

    public bool? EnLigne { get; set; }

    public int? ImageId { get; set; }

    public virtual ICollection<Ami> AmiUtilisateurId1Navigations { get; set; } = new List<Ami>();

    public virtual ICollection<Ami> AmiUtilisateurId2Navigations { get; set; } = new List<Ami>();

    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();

    public virtual Image? Image { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();
}
