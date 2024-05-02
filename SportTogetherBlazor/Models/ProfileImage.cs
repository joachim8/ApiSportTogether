using System;
using System.Collections.Generic;
namespace SportTogetherBlazor.Models;
public partial class ProfileImage
{
    public int ImageId { get; set; }

    public int? UtilisateursId { get; set; }

    public string? Url { get; set; }

    public DateTime? Timestamp { get; set; }

    public string Type { get; set; } = null!;

    public virtual Utilisateur? Utilisateurs { get; set; }
}
