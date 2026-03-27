using GetComments.Models;

public class ApiReplyCommentsDto
{
    public double fetcherSucessfulResponseCount { get; set; }
    public ApiReplyCommentsDtoReply[] replies { get; set; }
    public string nextToken { get; set; }
}

public class ApiReplyCommentsDtoReply
{
    public string id { get; set; }
    public string reportingId { get; set; }
    public string inReplyToId { get; set; }
    public string commentId { get; set; }
    public DateTimeOffset dateCreated { get; set; }
    public ApiSharedAuthorDto author { get; set; }
    public string text { get; set; }
    public ApiSharedRatingDto rating { get; set; }
    public ApiSharedActionsDto actions { get; set; }
    public string status { get; set; }
}