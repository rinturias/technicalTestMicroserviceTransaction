using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yape.Transactions.Domain.Entities
{
    [Table("accounts")]
    public class Accounts
    {
        [Key]
        [Required]
        [Column("accountid")]
        public Guid AccountId { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("accountholder")]
        public string AccountHolder { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        [Column("accountnumber")]
        public string AccountNumber{ get; set; } = string.Empty;

        [Required]
        [Column("balance", TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; } = 0;
    }
}
