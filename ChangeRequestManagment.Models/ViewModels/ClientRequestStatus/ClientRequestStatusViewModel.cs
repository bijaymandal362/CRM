using System;

namespace ChangeRequestManagment.Models.ViewModels.ClientRequestStatus
{
    public class ClientRequestStatusViewModel
    {
        public int ChangeRequestId { get; set; }
        public int ChangeRequestStatusId { get; set; }
        public int  ChangeRequestStatusListItemId { get; set; }
        public string ClientName { get; set; }
        public string ProjectName { get; set; }
        public string ChangeRequestNumber { get; set; }
        public string ModuleName { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string Status { get; set; }

        public int InsertPersonId { get; set; }
        public DateTime InsertDate { get; set; } = DateTime.Now;
        public int? UpdatePersonId { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.Now;
    }
}