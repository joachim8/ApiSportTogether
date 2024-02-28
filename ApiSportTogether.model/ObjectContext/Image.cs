using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Image
{
    public int ImagesId { get; set; }

    public int? UtilisateurId { get; set; }

    public string? Url { get; set; }

    public string? Type { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();

    public virtual ICollection<Publication> Publications { get; set; } = new List<Publication>();

    public virtual Utilisateur? Utilisateur { get; set; }

    public virtual ICollection<Utilisateur> Utilisateurs { get; set; } = new List<Utilisateur>();
}
