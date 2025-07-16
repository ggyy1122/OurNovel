using System;
using System.Text.Json.Serialization;
using OurNovel.Models;

namespace OurNovel.Models
{
    public class Management : IEntity<int>
    {
        public int ManagementId { get; set; }               // MANAGEMENT_ID 主键
        int IEntity<int>.Id
        {
            get => ManagementId;
            set => ManagementId = value;
        }

        public int ManagerId { get; set; }                  // MANAGER_ID 外键
        public string? Result { get; set; }                 // RESULT 可空
        public DateTime? Time { get; set; }                 // TIME 可空（数据库默认 SYSDATE）

        [JsonIgnore]
        public Manager? Manager { get; set; }

    }
}
