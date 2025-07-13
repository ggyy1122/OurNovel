using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace OurNovel.Services.FileStorage.Interfaces
{
    /// <summary>
    /// 文件存储服务接口（抽象上传、删除等操作）
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// 上传文件并返回可访问的URL
        /// </summary>
        /// <param name="file">前端上传的文件</param>
        /// <param name="subFolder">上传文件保存的子目录名（不带斜杠），例如 "avatars" 或 "book"</param>
        /// <returns>文件访问URL（如 "/avatars/xxx.jpg"）</returns>
        Task<string> UploadAsync(IFormFile file, string subFolder);

        /// <summary>
        /// 根据文件URL删除指定子目录下的文件
        /// </summary>
        /// <param name="fileUrl">文件的完整URL或相对路径</param>
        /// <param name="subFolder">文件所在的子目录名（不带斜杠）</param>
        void Delete(string fileUrl, string subFolder);




    }
}