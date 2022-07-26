using System;

namespace ChangeRequestManagement.Entities
{
    public abstract class BaseEntity
    {
        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; }
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}