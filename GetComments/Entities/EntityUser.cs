using Newtonsoft.Json;

namespace GetComments.Entities;

public class EntityUser
{
    [JsonProperty("id")]
    public required string Id { get; set; } = string.Empty;
    
    public required string DisplayName { get; set; } = string.Empty;
    
    public ICollection<EntityComment>? Comments { get; set; }
}