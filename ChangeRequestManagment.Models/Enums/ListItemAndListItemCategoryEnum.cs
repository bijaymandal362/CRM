namespace ChangeRequestManagment.Models.Enums
{
    public class ListItemAndListItemCategoryEnum
    {
        public enum ListItemCategoryEnum
        {
            ChangeRequestType = 1,
            Priority = 2,
            ChangeRequestStatus = 3
        }

        public enum ChangeRequestTypeEnum
        {
            Enhancement = 1,
            Defect = 2
        }

        public enum PriorityEnum
        {
            Low = 3,
            Medium = 4,
            High = 5,
            Critical = 6
        }

        public enum ChangeRequestStatusEnum
        {
            Pending = 7,
            Approved = 8,
            Development = 9,
            Testing = 10,
            Staging = 11,
            Delivered = 12
        }
    }
}