using OurNovel.Models;

namespace OurNovel.Services
{
    public interface ILikesService
    {
        Task LikeAsync(int commentId, int readerId);
        Task UnlikeAsync(int commentId, int readerId);
        Task<bool> IsLikedAsync(int commentId, int readerId);
        Task<int> GetLikeCountAsync(int commentId);
    }
}
