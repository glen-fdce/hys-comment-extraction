using GetComments.Entities;
using GetComments.Services;

namespace GetComments;

public class MainExecution
{
    private readonly BBCCommunicationService _bbcCommunicationService;
    private readonly DatabaseService _databaseService;

    public MainExecution(BBCCommunicationService bbcCommunicationService,
        DatabaseService databaseService)
    {
        _bbcCommunicationService = bbcCommunicationService;
        _databaseService = databaseService;
    }

    public async Task RunAsync()
    {
        Console.WriteLine("*** RUNNING ***");
        
        bool canLoadMoreComments = true;
        string nextCommentsToken = string.Empty;
        
        int infoCommentCounter = 1;
        while (canLoadMoreComments)
        {
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(!string.IsNullOrWhiteSpace(nextCommentsToken)
                ? $"Getting comments page {infoCommentCounter} (using next token {nextCommentsToken.Substring(0, 5)})"
                : $"Getting comments page {infoCommentCounter}");
            Console.ResetColor();
            await Task.Delay(TimeSpan.FromSeconds(2));
            infoCommentCounter++;
            
            var commentsPage = await GetComments(string.IsNullOrEmpty(nextCommentsToken) ? null : nextCommentsToken);
            
            if (commentsPage is null)
            {
                continue;
            }

            foreach (var comment in commentsPage.comments)
            {
                EntityUser commentAuthor = await _databaseService.CreateUser(comment.author);
                await _databaseService.CreateComment(comment, commentAuthor);
                
                if (comment.totalReplies > 0)
                {
                    bool canLoadMoreReplies = true;
                    string nextRepliesToken = string.Empty;
                    
                    int infoReplyCounter = 1;
                    while (canLoadMoreReplies)
                    {
                        Console.BackgroundColor = ConsoleColor.DarkCyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(!string.IsNullOrWhiteSpace(nextRepliesToken)
                            ? $"Getting reply page {infoReplyCounter} for reply {comment.id} (using next token {nextRepliesToken.Substring(0, 5)})"
                            : $"Getting reply page {infoReplyCounter} for reply {comment.id}");
                        Console.ResetColor();
                        await Task.Delay(TimeSpan.FromSeconds(2));
                        infoReplyCounter++;
                        
                        var repliesPage = await GetReplies(comment.id,
                            string.IsNullOrWhiteSpace(nextRepliesToken)
                                ? null
                                : nextRepliesToken);

                        if (repliesPage is null || repliesPage.replies.Length == 0)
                        {
                            continue;
                        }
                        
                        foreach (var reply in repliesPage.replies)
                        {
                            EntityUser replyAuthor = await _databaseService.CreateUser(reply.author);
                            await _databaseService.CreateReply(reply, replyAuthor);
                        }
                        
                        nextRepliesToken = repliesPage.nextToken;
                        canLoadMoreReplies = !string.IsNullOrWhiteSpace(nextRepliesToken);
                    }
                }
            }
            
            nextCommentsToken = commentsPage.nextToken;
            canLoadMoreComments = !string.IsNullOrWhiteSpace(nextCommentsToken) && commentsPage.canLoadMore;
        }
        
        Console.WriteLine("*** END ***");
        
        
        async Task<ApiCommentsDto?> GetComments(string? nextToken = null)
        {
            string requestParams = """{"requestType":"comments"##NEXT_TOKEN##}""";

            if (nextToken is null)
            {
                requestParams = requestParams.Replace("##NEXT_TOKEN##", string.Empty);
            }
            else
            {
                requestParams = requestParams.Replace(
                    "##NEXT_TOKEN##",
                    $",\"nextToken\":\"{nextToken}\"");
            }
            string encodedRequestParams = Uri.EscapeDataString(requestParams); 
            return await _bbcCommunicationService.HttpGetAsync<ApiCommentsDto>(encodedRequestParams);
        }
        
        async Task<ApiReplyCommentsDto?> GetReplies(string commentId, string? nextToken = null)
        {
            string requestParams = $$"""{"requestType":"replies","commentId":"{{commentId}}","pageSize":50##NEXT_TOKEN##}""";
            
            if (nextToken is null)
            {
                requestParams = requestParams.Replace("##NEXT_TOKEN##", string.Empty);
            }
            else
            {
                requestParams = requestParams.Replace(
                    "##NEXT_TOKEN##",
                    $",\"nextToken\":\"{nextToken}\"");
            }

            string encodedRequestParams = Uri.EscapeDataString(requestParams); 
            return await _bbcCommunicationService.HttpGetAsync<ApiReplyCommentsDto>(encodedRequestParams);
        }
        
    }
}