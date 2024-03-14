using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Participation
{
    public int ParticipationsId { get; set; }

    public int? UtilisateurId { get; set; }

    public int? AnnonceId { get; set; }

    public int? GroupeId { get; set; }

    public DateTime? DateParticipation { get; set; }
}
