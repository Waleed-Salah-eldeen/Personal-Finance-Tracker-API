namespace PersonalFinanceTracker.API.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AccountDto, Account>();
            CreateMap<Account, AccountDetailsDto>(); 
            CreateMap<TransactionDto, Transaction>();
            CreateMap<Transaction, TransactionDetailsDto>();
            
        }
    }
}
