using System;
using System.Collections.Generic;

namespace SportTogetherBlazor.Models;

public partial class VuMessage
{
    public int IdMessageVu { get; set; }

    public bool Vu { get; set; }

    public int? UtilisateurId { get; set; }

    public int? messages_id { get; set; }

    public virtual Message? IdMessageNavigation { get; set; } = null!;

    public virtual Utilisateur? Utilisateur { get; set; } = null!;
}
