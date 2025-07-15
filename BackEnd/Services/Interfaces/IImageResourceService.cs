using OurNovel.Repositories;

namespace OurNovel.Services.Interfaces
{
    public interface IImageResourceService
    {
        Task<string> UploadImageAsync<TEntity, TKey>(
            TKey id,
            IFormFile imageFile,
            string imagePropertyName,
            IRepository<TEntity, TKey> repository)
            where TEntity : class;
    }

}
