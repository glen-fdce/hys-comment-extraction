using GetComments.Models;

public class ApiCommentsDto
{
    public double fetcherSucessfulResponseCount { get; set; }
    public string title { get; set; }
    public ApiCommentsDtoIdcta idcta { get; set; }
    public string siteName { get; set; }
    public ApiCommentsDtoModeration moderation { get; set; }
    public ApiCommentsDtoComment[] comments { get; set; }
    public int totalPostsCount { get; set; }
    public int totalCommentsCount { get; set; }
    public bool canLoadMore { get; set; }
    public string nextToken { get; set; }
    public int commentTextLimit { get; set; }
    public string env { get; set; }
    public bool isClosed { get; set; }
    public bool createForum { get; set; }
    public ApiCommentsDtoConfig config { get; set; }
    public string sortOrder { get; set; }
    public object notableUser { get; set; }
}

public class ApiCommentsDtoIdcta
{
    public string policy { get; set; }
}

public class ApiCommentsDtoModeration
{
    public string rulesPageUrl { get; set; }
    public string status { get; set; }
}

public class ApiCommentsDtoComment
{
    public string id { get; set; }
    public string reportingId { get; set; }
    public DateTimeOffset dateCreated { get; set; }
    public ApiSharedAuthorDto author { get; set; }
    public string text { get; set; }
    public ApiSharedRatingDto rating { get; set; }
    public ApiCommentsDtoReply[] replies { get; set; }
    public int totalReplies { get; set; }
    public ApiSharedActionsDto actions { get; set; }
    public string status { get; set; }
}

public class ApiCommentsDtoReply
{
    public string id { get; set; }
    public string reportingId { get; set; }
    public string commentId { get; set; }
    public string inReplyToId { get; set; }
    public DateTimeOffset dateCreated { get; set; }
    public ApiSharedAuthorDto author { get; set; }
    public string text { get; set; }
    public ApiSharedRatingDto rating { get; set; }
    public ApiSharedActionsDto actions { get; set; }
    public string status { get; set; }
}


public class ApiCommentsDtoConfig
{
    public ApiCommentsDtoFeatureToggles featureToggles { get; set; }
    public string theme { get; set; }
    public string subtheme { get; set; }
}

public class ApiCommentsDtoFeatureToggles
{
    public bool downRatings { get; set; }
    public bool sort { get; set; }
}

