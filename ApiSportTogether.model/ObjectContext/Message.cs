using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Message
{
    public int Id { get; set; }

    public int? GroupeId { get; set; }

    public int? UtilisateurId { get; set; }

    public string? Contenu { get; set; }

    public DateTime? Timestamp { get; set; }
}
