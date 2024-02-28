using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Ami
{
    public int AmisId { get; set; }

    public int? UtilisateurId1 { get; set; }

    public int? UtilisateurId2 { get; set; }

    public DateTime? DateAjout { get; set; }

    public virtual Utilisateur? UtilisateurId1Navigation { get; set; }

    public virtual Utilisateur? UtilisateurId2Navigation { get; set; }
}
