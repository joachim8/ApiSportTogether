using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SportTogetherBlazor.Models;

public partial class PublicationImage
{
    [JsonPropertyName("ImageId")]
    public int ImageId { get; set; }
    [JsonPropertyName("PublicationsId")]
    public int? PublicationsId { get; set; }
    [JsonPropertyName("Url")]
    public string? Url { get; set; }
    [JsonPropertyName("Timestamp")]
    public DateTime? Timestamp { get; set; }
    [JsonPropertyName("Type")]
    public string Type { get; set; } = null!;
    [JsonPropertyName("Publications")]
    public virtual Publication? Publications { get; set; }
}
