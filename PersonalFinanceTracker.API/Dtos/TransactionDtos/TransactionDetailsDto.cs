namespace PersonalFinanceTracker.API.Dtos.TransactionDtos
{
    public class TransactionDetailsDto
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public TransactionType Type { get; set; }  // Income/Expense
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
