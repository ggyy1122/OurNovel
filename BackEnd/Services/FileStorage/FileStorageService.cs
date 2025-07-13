using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using OurNovel.Services.FileStorage.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace OurNovel.Services.FileStorage
{
    /// <summary>
    /// 本地文件存储服务（实现 IFileStorageService）
    /// </summary>
    public class LocalFileStorageService : IFileStorageService
    {
        private readonly FileStorageOptions _options;

        public LocalFileStorageService(IOptions<FileStorageOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// 上传文件到指定子目录（如 "avatars" 或 "book"）
        /// </summary>
        /// <param name="file">上传的文件</param>
        /// <param name="subFolder">子目录名，不带斜杠</param>
        /// <returns>返回公网访问 URL</returns>
        public async Task<string> UploadAsync(IFormFile file, string subFolder)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("文件不能为空");

            var fileExt = Path.GetExtension(file.FileName).ToLower();
            var fileName = $"{Guid.NewGuid()}{fileExt}";

            // 拼接本地存储路径
            var uploadPath = string.IsNullOrWhiteSpace(subFolder)
                ? _options.UploadPath
                : Path.Combine(_options.UploadPath, subFolder);
            Console.WriteLine($"[文件上传] 物理存储路径：{uploadPath}");
            Directory.CreateDirectory(uploadPath);
            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // 拼接访问 URL
            var baseUrl = _options.BaseUrl;
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            var url = string.IsNullOrWhiteSpace(subFolder)
                ? $"{baseUrl}{fileName}"
                : $"{baseUrl}{subFolder}/{fileName}";

            return url;
        }

        /// <summary>
        /// 删除指定子目录中的文件
        /// </summary>
        /// <param name="fileUrl">文件访问 URL</param>
        /// <param name="subFolder">子目录名，不带斜杠</param>
        public void Delete(string fileUrl, string subFolder)
        {
            var fileName = Path.GetFileName(fileUrl);
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("无效的文件URL");

            var uploadPath = string.IsNullOrWhiteSpace(subFolder)
                ? _options.UploadPath
                : Path.Combine(_options.UploadPath, subFolder);

            var filePath = Path.Combine(uploadPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
