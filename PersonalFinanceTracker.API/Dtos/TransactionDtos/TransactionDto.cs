namespace PersonalFinanceTracker.API.Dtos.TransactionDtos
{
    public class TransactionDto
    {
        public int AccountId { get; set; }
        public TransactionType Type { get; set; }  // Income/Expense
        public string? Description { get; set; }
        public decimal Amount { get; set; }
    }
}
