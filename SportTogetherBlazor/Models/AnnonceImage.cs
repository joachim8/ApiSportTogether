﻿using System;
using System.Collections.Generic;

namespace SportTogetherBlazor.Models;

public partial class AnnonceImage
{
    public int ImageId { get; set; }

    public int? AnnoncesId { get; set; }

    public string? Url { get; set; }

    public DateTime? Timestamp { get; set; }

    public virtual Annonce? Annonces { get; set; }
}
