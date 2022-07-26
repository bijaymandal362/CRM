using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChangeRequestManagement.Entities
{
    public class Client : BaseEntity
    {
        [Key]
        public int ClientId { get; set; }

        [ForeignKey(nameof(Company)), Required]
        public int CompanyId { get; set; }

        public Company Company { get; set; } 

        [ForeignKey(nameof(Person)), Required]
        public int PersonId { get; set; }

        public Person Person { get; set; }
    }
}
