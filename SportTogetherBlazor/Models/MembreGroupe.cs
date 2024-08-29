using System.Collections.Generic;

namespace SportTogetherBlazor.Models;

public partial class MembreGroupe
{
    public int IdMembreGroupe { get; set; }

    public int UtilisateurId { get; set; }

    public int GroupeId { get; set; }

    public string Role { get; set; } = null!;

    public virtual Groupe Groupe { get; set; } = null!;

    public virtual Utilisateur Utilisateur { get; set; } = null!;
}
