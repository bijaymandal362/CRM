using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class ChangeRequestDocument : BaseEntity
    {
        [Key]
        public int ChangeRequestDocumentId { get; set; }

        [ForeignKey(nameof(ChangeRequest))]
        public int ChangeRequestId { get; set; }

        public ChangeRequest ChangeRequest { get; set; }

        [ForeignKey(nameof(ChangeRequestStatus))]
        public int ChangeRequestStatusId { get; set; }

        public ChangeRequestStatus ChangeRequestStatus { get; set; }
        public string DocumentPath { get; set; }
    }
}