using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models
{
    /// <summary>
    /// �½�ʵ���࣬ʹ�ø���������NovelId + ChapterId��
    /// </summary>
    public class Chapter
    {   
        /// ����С˵ID����������֮һ��
        public int NovelId { get; set; }

        /// �½ڱ�ţ���������֮һ��
        public int ChapterId { get; set; }

        /// ����������ԣ��½�����С˵��������ڣ�
       public Novel Novel { get; set; } = null!;

        /// ���⣬��󳤶�40������Ϊ��
        [Required]
        [MaxLength(40)]
        public string Title { get; set; } = null!;

        /// �½����ݣ����ı���
        public string? Content { get; set; }

        /// ����������Ϊ��
        [Required]
        public int WordCount { get; set; }

        /// ǧ�ֵ��ۣ�Ĭ��0.50��������λС��
        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerKilo { get; set; } = 0.50m;

        /// ����۸��Զ����㣨��ӳ���ֶΣ�
        [NotMapped]
        public decimal CalculatedPrice =>
            Math.Round((WordCount / 1000m) * PricePerKilo, 2);

        /// �Ƿ��շѣ���/��
        public string? IsCharged { get; set; }

        /// ����ʱ��
        public DateTime? PublishTime { get; set; }

        /// ״̬��ͨ�� / �����Ĭ��ͨ����
        public string Status { get; set; } = "ͨ��";
    }
}
