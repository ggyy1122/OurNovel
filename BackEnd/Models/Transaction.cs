using OurNovel.Models;
namespace OurNovel.Models
{
    /// <summary>
    /// 表示用户的交易记录实体
    /// </summary>
    public class Transaction : IEntity<int>
    {
        int IEntity<int>.Id
        {
            get => TransactionId;
            set => TransactionId = value;
        }
        public int ReaderId { get; set; }
        public int TransactionId { get; set; }
        public string TransType { get; set; }

        public decimal Amount { get; set; }

        public DateTime? Time { get; set; }

        public enum TransactionType           //交易类型枚举（对应数据库中的 CHECK 约束）
        {
            打赏,
            充值,
            解锁章节
        }
    }
}