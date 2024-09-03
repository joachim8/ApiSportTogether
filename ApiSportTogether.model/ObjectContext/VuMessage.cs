using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class VuMessage
{
    public int IdMessageVu { get; set; }

    public bool Vu { get; set; }

    public int? UtilisateurId { get; set; }

    public int? messages_id { get; set; }

    public virtual Message? IdMessageNavigation { get; set; }

    public virtual Utilisateur? Utilisateur { get; set; }
}
