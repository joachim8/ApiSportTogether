using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiSportTogether.model.ObjectVue
{
    public partial class AmiVue
    {
        [JsonPropertyName("Pseudo")]
        public string Pseudo { get; set; } = null!;

        [JsonPropertyName("UrlProfilImage")]
        public string? UrlProfilImage { get; set; }
    }
}
