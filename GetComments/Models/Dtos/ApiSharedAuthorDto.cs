namespace GetComments.Models;

public class ApiSharedAuthorDto
{
    public string userId { get; set; }
    public string displayName { get; set; }
    public bool isNotable { get; set; }
    public object description { get; set; }
    public object avatarUrl { get; set; }
}