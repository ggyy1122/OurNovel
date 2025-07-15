using OurNovel.Repositories;
using OurNovel.Services.Interfaces;

namespace OurNovel.Services
{
    public class ImageResourceService : IImageResourceService
    {
        private readonly IOssService _ossService;

        public ImageResourceService(IOssService ossService)
        {
            _ossService = ossService;
        }

        public async Task<string> UploadImageAsync<TEntity, TKey>(
            TKey id,
            IFormFile imageFile,
            string imagePropertyName,
            IRepository<TEntity, TKey> repository)
            where TEntity : class
        {
            if (imageFile == null || imageFile.Length == 0)
                throw new ArgumentException("图片文件不能为空");

            string imageUrl = null;

            try
            {
                // 上传图片，返回完整URL
                imageUrl = await _ossService.UploadFileAsync(imageFile);
                var fileName = imageUrl.Split('/').Last();

                // 查找实体
                var entity = await repository.GetByIdAsync(id);
                if (entity == null)
                    throw new Exception($"未找到ID为 {id} 的实体 {typeof(TEntity).Name}");

                // 通过反射设置图片URL字段值
                var prop = typeof(TEntity).GetProperty(imagePropertyName);
                if (prop == null)
                    throw new Exception($"{typeof(TEntity).Name} 没有 {imagePropertyName} 属性");

                prop.SetValue(entity, fileName);
                await repository.UpdateAsync(entity);

                return imageUrl;
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var fileName = imageUrl.Split('/').Last();
                    _ossService.DeleteFile(fileName);
                }
                throw;
            }
        }
    }

}
