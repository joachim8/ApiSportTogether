using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Groupe
{
    public int Id { get; set; }

    public int? AnnonceId { get; set; }

    public DateTime? DateCreation { get; set; }

    public DateTime? DateSuppression { get; set; }

    public virtual Annonce? Annonce { get; set; }
}
