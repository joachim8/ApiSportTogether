using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Publication
{
    public int PublicationsId { get; set; }

    public int UtilisateurId { get; set; }

    public string? Contenu { get; set; }

    public int ImageId { get; set; }

    public virtual Image Image { get; set; } = null!;

    public virtual Utilisateur Utilisateur { get; set; } = null!;
}
