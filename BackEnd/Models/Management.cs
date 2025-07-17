using System;
using System.Text.Json.Serialization;
using OurNovel.Models;

namespace OurNovel.Models
{
    public class Management : IEntity<int>
    {
        public int ManagementId { get; set; }               // MANAGEMENT_ID ����
        int IEntity<int>.Id
        {
            get => ManagementId;
            set => ManagementId = value;
        }

        public int ManagerId { get; set; }                  // MANAGER_ID ���
        public string? Result { get; set; }                 // RESULT �ɿ�
        public DateTime? Time { get; set; }                 // TIME �ɿգ����ݿ�Ĭ�� SYSDATE��

        [JsonIgnore]
        public Manager? Manager { get; set; }

    }
}
