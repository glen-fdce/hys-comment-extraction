namespace GetComments.Options;

public sealed class HysOptions
{
    public const string PropertyName = "HYS";

    public string ApiKey { get; set; } = string.Empty;
    public string ForumId { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = string.Empty;
    public string UrlPrefix { get; set; } = string.Empty;
}

