using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aliyun.OSS;
using Aliyun.OSS.Common; // 新增引用
using System.Net;

namespace OurNovel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : ControllerBase
    {
        private readonly string endpoint = "oss-cn-hangzhou.aliyuncs.com";
        private readonly string accessKeyId = "LTAI5tEvZTZqgdHsTpRjAZDZ";
        private readonly string accessKeySecret = "joqOQwH87R7FPU5lR1QVWHpha7P1Ay";
        private readonly string bucketName = "novelprogram123";

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file)
        {
            // 初始化客户端（无需释放）
            var client = new OssClient(endpoint, accessKeyId, accessKeySecret, new ClientConfiguration
            {
                ConnectionTimeout = 5000,
                MaxErrorRetry = 2
            });

            try
            {
                Console.WriteLine("==== 开始处理文件 ====");

                if (file == null || file.Length == 0)
                    return BadRequest("文件无效");

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                using var stream = file.OpenReadStream();



                await Task.Run(() => client.PutObject(bucketName, fileName, stream));

                var url = $"https://{bucketName}.{endpoint}/{fileName}";
                return Ok(new { url });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! 上传异常: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // ✅ 新增 删除接口
        [HttpDelete("delete")]
        public IActionResult DeleteImage([FromQuery] string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                return BadRequest("文件名不能为空");

            var client = new OssClient(endpoint, accessKeyId, accessKeySecret, new ClientConfiguration
            {
                ConnectionTimeout = 5000,
                MaxErrorRetry = 2
            });

            try
            {
                client.DeleteObject(bucketName, fileName);
                Console.WriteLine($"✅ 删除成功: {fileName}");
                return Ok(new { message = "删除成功" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ 删除失败: {ex.GetType().Name} - {ex.Message}");
                return StatusCode(500, new { error = ex.Message });
            }
        }

     

    }
}