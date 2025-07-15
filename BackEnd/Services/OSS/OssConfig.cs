namespace OurNovel.Services.OSS
{
    public class OssConfig
    {
        public string Endpoint { get; set; } = null!;
        public string AccessKeyId { get; set; } = null!;
        public string AccessKeySecret { get; set; } = null!;
        public string BucketName { get; set; } = null!;
    }

}
