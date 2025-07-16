using Microsoft.EntityFrameworkCore;
using OurNovel.Data;
using OurNovel.Models;

public class CommentReplyRepository : ICommentReplyRepository
{
    private readonly AppDbContext _context;

    public CommentReplyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CommentReply entity)
    {
        _context.CommentReplies.Add(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<CommentReply?> GetByCommentIdAsync(int commentId)
    {
        return await _context.CommentReplies
            .FirstOrDefaultAsync(r => r.CommentId == commentId);
    }

    public async Task<IEnumerable<CommentReply>> GetRepliesByParentIdAsync(int parentCommentId)
    {
        return await _context.CommentReplies
            .Where(r => r.PreComId == parentCommentId)
            .ToListAsync();
    }
    public async Task<IEnumerable<CommentReply>> GetAllAsync()
    {
        return await _context.CommentReplies.ToListAsync();
    }
    public async Task DeleteAsync(int commentId)
    {
        var reply = await _context.CommentReplies.FirstOrDefaultAsync(r => r.CommentId == commentId);
        if (reply != null)
        {
            _context.CommentReplies.Remove(reply);
            await _context.SaveChangesAsync();
        }
    }

}
