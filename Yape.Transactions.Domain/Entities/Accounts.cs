using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yape.Transactions.Domain.Entities
{
    [Table("Accounts")]
    public class Accounts
    {
        [Key]
        public Guid AccountId { get; set; }

        [Required]
        [MaxLength(100)]
        public string AccountHolder { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;
    }
}
