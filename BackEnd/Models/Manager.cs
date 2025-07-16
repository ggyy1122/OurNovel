namespace OurNovel.Models
{
    public class Manager : IEntity<int>
    {
        public int ManagerId { get; set; }                  // MANAGER_ID 主键
        int IEntity<int>.Id
        {
            get => ManagerId;
            set => ManagerId = value;
        }
        public string ManagerName { get; set; } = null!;    // MANAGER_NAME 非空
        public string Password { get; set; } = null!;       // PASSWORD 非空
    }
}
