namespace OurNovel.DTOs
{
    public class PurchaseResultDto
    {
        public int Success { get; set; }  // �� int ��� bool��1 ��ʾ�ɹ���0 ��ʾʧ��
        public string Message { get; set; } = string.Empty;
    }
}
