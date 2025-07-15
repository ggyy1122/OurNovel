namespace OurNovel.Services.Interfaces
{
    /// <summary>
    /// OSS服务接口定义
    /// </summary>
    public interface IOssService
    {
        /// <summary>
        /// 异步上传文件到OSS
        /// </summary>
        /// <param name="file">要上传的文件</param>
        /// <returns>返回文件在OSS上的访问URL</returns>
        Task<string> UploadFileAsync(IFormFile file);
        /// <summary>
        /// 从OSS删除文件
        /// </summary>
        /// <param name="fileName">要删除的文件名</param>
        void DeleteFile(string fileName);
    }
}
