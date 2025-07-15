using System.ComponentModel.DataAnnotations.Schema;

namespace OurNovel.Models
{
    public class Category : IEntity<string>
    {
        [Column("CATEGORY_NAME")]
        public string CategoryName { get; set; } = null!;

        [NotMapped]
        public string Id
        {
            get => CategoryName;
            set => CategoryName = value;
        }
    }
}
