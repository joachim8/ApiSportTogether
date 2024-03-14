using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class SportFavori
{
    public int SportFavoriId { get; set; }

    public int SportsId { get; set; }

    public int UtilisateursId { get; set; }

    public virtual Sport Sports { get; set; } = null!;

    public virtual Utilisateur Utilisateurs { get; set; } = null!;
}
