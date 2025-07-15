using Aliyun.OSS;
using OurNovel.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace OurNovel.Services.OSS
{
    /// <summary>
    /// OSS服务实现类
    /// </summary>
    public class OssService : IOssService
    {
        private readonly OssClient client;    // 阿里云OSS客户端实例
        private readonly string bucketName;   // OSS存储桶名称
        private readonly string endpoint;     // OSS端点地址
        /// <summary>
        /// 构造函数，通过依赖注入获取配置
        /// </summary>
        /// <param name="config">OSS配置项</param>
        public OssService(Microsoft.Extensions.Options.IOptions<OssConfig> config)
        {
            var ossConfig = config.Value;  // 获取配置值
            client = new OssClient(ossConfig.Endpoint, ossConfig.AccessKeyId, ossConfig.AccessKeySecret); // 初始化OSS客户端
            bucketName = ossConfig.BucketName; // 设置存储桶名称
            endpoint = ossConfig.Endpoint;   // 保存Endpoint用于后续构造URL
        }
        /// <summary>
        /// 异步上传文件到OSS
        /// </summary>
        /// <param name="file">要上传的文件</param>
        /// <returns>返回文件在OSS上的访问URL</returns>
        public async Task<string> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("文件无效");
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // 生成唯一的文件名，保留原始文件扩展名
            using var stream = file.OpenReadStream();
            await Task.Run(() => client.PutObject(bucketName, fileName, stream));
            // 构造并返回文件的公开访问URL
            // 格式: https://{bucketName}.{endpoint}/{fileName}
            return $"https://{bucketName}.{endpoint}/{fileName}";
        }
        /// <summary>
        /// 从OSS删除文件
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        public void DeleteFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("文件名不能为空");
            client.DeleteObject(bucketName, fileName);// 调用OSS客户端删除指定文件
        }
    }

}
