using System.ComponentModel.DataAnnotations;
using GetComments.Attributes;

namespace GetComments.Options;

public sealed class DatabaseOptions
{
    public const string PropertyName = "Database";
    
    [Required, NotEmptyOrWhitespace]
    public string Type { get; set; } = string.Empty;
    
    [Required, NotEmptyOrWhitespace]
    public string ConnectionString { get; set; } = string.Empty;
    
    
}