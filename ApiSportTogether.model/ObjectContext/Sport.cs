using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class Sport
{
    public int SportsId { get; set; }

    public string? Nom { get; set; }

    public virtual ICollection<Annonce> Annonces { get; set; } = new List<Annonce>();
}
