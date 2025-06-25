namespace PersonalFinanceTracker.API.Core.Models
{
    [Index(nameof(Name),IsUnique = true)]
    public class Account
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;  // e.g. "Main Account", "Travel Fund"

        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
