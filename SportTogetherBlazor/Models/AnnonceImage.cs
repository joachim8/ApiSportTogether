using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models;

public partial class AnnonceImage
{
    [JsonPropertyName("ImageId")]
    public int ImageId { get; set; }
    [JsonPropertyName("AnnoncesId")]
    public int? AnnoncesId { get; set; }
    [JsonPropertyName("Url")]
    public string? Url { get; set; }
    [JsonPropertyName("Timestamp")]
    public DateTime? Timestamp { get; set; }
    [JsonPropertyName("Annonces")]
    public virtual Annonce? Annonces { get; set; }
}
