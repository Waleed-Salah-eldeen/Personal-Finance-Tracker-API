namespace PersonalFinanceTracker.API.Dtos.AccountDtos
{
    public class AccountDto
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;  // e.g. "Main Account", "Travel Fund"

        public decimal Balance { get; set; }
    }
}
