namespace PersonalFinanceTracker.API.Dtos.AccountDtos
{
    public class AccountDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public decimal Balance { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
