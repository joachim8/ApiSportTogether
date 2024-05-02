using System;
using System.Collections.Generic;

namespace ApiSportTogether.model.ObjectContext;

public partial class PublicationImage
{
    public int ImageId { get; set; }

    public int? PublicationsId { get; set; }

    public string? Url { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Publication? Publications { get; set; }
}
