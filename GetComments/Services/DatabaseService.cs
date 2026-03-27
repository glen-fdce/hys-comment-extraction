using GetComments.Data;
using GetComments.Entities;
using GetComments.Models;
using Microsoft.EntityFrameworkCore;

namespace GetComments.Services;

public class DatabaseService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;

    public DatabaseService(IDbContextFactory<ApplicationDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<EntityUser> CreateUser(ApiSharedAuthorDto user)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        EntityUser? existingUser = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.userId);

        if (existingUser is not null)
        {
            return existingUser;
        }
        
        EntityUser newUser = new EntityUser
        {
            Id = user.userId,
            DisplayName = user.displayName
        };
        dbContext.Users.Add(newUser);
        await dbContext.SaveChangesAsync();
        return newUser;
    }
    
    public async Task CreateComment(ApiCommentsDtoComment comment, EntityUser user)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        bool commentExists = await dbContext.Comments.AnyAsync(c => c.Id == comment.id);
        if (!commentExists)
        {
            EntityComment newComment = new EntityComment
            {
                Id = comment.id,
                CommentType = CommentType.Main,
                CreatedDate = comment.dateCreated.UtcDateTime,
                ParentCommentId = comment.id,
                RatingNegative = comment.rating.negative,
                RatingPositive = comment.rating.positive,
                Text = comment.text,
                UserId = user.Id
            };
            
            dbContext.Comments.Add(newComment);
            await dbContext.SaveChangesAsync();
        }
    }
    
    public async Task CreateReply(ApiReplyCommentsDtoReply reply, EntityUser user)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
        bool replyExists = await dbContext.Comments.AnyAsync(r => r.Id == reply.id);
        if (!replyExists)
        {
            EntityComment newComment = new EntityComment
            {
                Id = reply.id,
                CommentType = reply.inReplyToId == reply.commentId ? CommentType.ReplyToMain : CommentType.ReplyToReply,
                CreatedDate = reply.dateCreated.UtcDateTime,
                ParentCommentId = reply.commentId,
                InReplyToId = reply.inReplyToId,
                RatingNegative = reply.rating.negative,
                RatingPositive = reply.rating.positive,
                Text = reply.text,
                UserId = user.Id
            };
            
            dbContext.Comments.Add(newComment);
            await dbContext.SaveChangesAsync();
        }
    }
}