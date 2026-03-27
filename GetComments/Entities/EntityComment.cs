using Newtonsoft.Json;

namespace GetComments.Entities;

public class EntityComment
{
    [JsonProperty("id")]
    public required string Id { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public required string UserId { get; set; } = string.Empty;
    public EntityUser? User { get; set; }
    public CommentType CommentType { get; set; }
    public required string ParentCommentId { get; set; } = string.Empty;
    public string? InReplyToId { get; set; }
    public int RatingPositive { get; set; }
    public int RatingNegative { get; set; }
    public required string Text { get; set; } = string.Empty;
}

public enum CommentType
{
    Main,
    ReplyToMain,
    ReplyToReply
}