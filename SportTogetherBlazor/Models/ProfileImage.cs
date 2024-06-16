using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
namespace SportTogetherBlazor.Models;
public partial class ProfileImage
{
    [JsonPropertyName("ImageId")]
    public int ImageId { get; set; }
    [JsonPropertyName("UtilisateursId")]
    public int? UtilisateursId { get; set; }
    [JsonPropertyName("Url")]
    public string? Url { get; set; }
    [JsonPropertyName("Timestamp")]
    public DateTime? Timestamp { get; set; }
    [JsonPropertyName("Type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("Utilisateurs")]
    public virtual Utilisateur? Utilisateurs { get; set; }
}
