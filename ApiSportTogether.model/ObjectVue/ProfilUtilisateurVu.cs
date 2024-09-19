using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiSportTogether.model.ObjectVue
{
    public class ProfilUtilisateurVu
    {
        [JsonPropertyName("UtilisateursId")]
        public int UtilisateursId { get; set; }
        [JsonPropertyName("Nom")]
        public string Nom { get; set; } = null!;
        [JsonPropertyName("Prenom")]
        public string Prenom { get; set; } = null!;
        [JsonPropertyName("Pseudo")]
        public string? Pseudo { get; set; }
        [JsonPropertyName("Genre")]
        public string? Genre { get; set; }
        [JsonPropertyName("Age")]
        public int? Age { get; set; }
        [JsonPropertyName("Ville")]
        public string? Ville { get; set; }
        [JsonPropertyName("Description")]
        public string? Description { get; set; }
        [JsonPropertyName("NombreAuteurAnnonce")]
        public int? NombreAuteurAnnonce { get; set; }
        [JsonPropertyName("NombreAnnonceEffectuer")]
        public int? NombreAnnonceEffectuer { get; set; }
        [JsonPropertyName("NoteMoyenneDesAnnonces")]
        public decimal? NoteMoyenneDesAnnonces { get; set; }
        [JsonPropertyName("AnnonceEffectuerParMoisMoyenne")]
        public decimal? AnnonceEffectuerParMoisMoyenne { get; set; }
        [JsonPropertyName("TopTroisSport")]
        public Array TopTroisSport { get; set; }
        [JsonPropertyName("urlProfilImage")]
        public string? urlProfilImage { get; set; }
    }
}
