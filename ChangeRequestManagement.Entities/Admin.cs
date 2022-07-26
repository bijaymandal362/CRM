using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class Admin : BaseEntity
    {
        [Key]
        public int AdminId { get; set; }

        [ForeignKey(nameof(Person)), Required]
        public int PersonId { get; set; }

        public Person Person { get; set; }
    }
}
