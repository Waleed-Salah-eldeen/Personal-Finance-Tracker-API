using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using PersonalFinanceTracker.API.Core.Models;

namespace PersonalFinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public TransactionsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransactionDetailsDto>>> GetTransactions(
        [FromQuery] int? accountId)
        {
            IQueryable<Transaction> query = _context.Transactions.Include(t => t.Account)
                                                    .OrderByDescending(t => t.Date);

            if (accountId.HasValue)   // If the user needs the Transactions of a specific account
            {
                var isValidAccountId = _context.Accounts.Any(a => a.Id == accountId);
                if (!isValidAccountId)
                    return BadRequest("Can not find an account with this id");
                query = query.Where(t => t.AccountId == accountId);
            }

            var transactions = await query.AsNoTracking().ToListAsync();

            var transactionsDetailsList = _mapper.Map<IEnumerable<TransactionDetailsDto>>(transactions);
            return Ok(transactionsDetailsList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionDetailsDto>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions
                                            .Include(t => t.Account)
                                            .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            return Ok(_mapper.Map<TransactionDetailsDto>(transaction));
        }

        [HttpPost]
        public async Task<ActionResult<TransactionDetailsDto>> CreateTransaction(
        [FromBody] TransactionDto dto)
        {
            var account = await _context.Accounts.FindAsync(dto.AccountId);
            if (account is null)
                return BadRequest("Can not find an account with this id");

            if(dto.Type == TransactionType.Expense && dto.Amount > account.Balance)
                return BadRequest("You do not have enough balance to complete this transaction.");

            var transaction = _mapper.Map<Transaction>(dto);

            account.Balance += transaction.Type == TransactionType.Income
                                ? transaction.Amount
                                : -transaction.Amount;

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<TransactionDetailsDto>(transaction));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Transaction>> DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions
                                             .Include(t => t.Account)
                                             .FirstOrDefaultAsync(t => t.Id == id);

            if (transaction == null)
                return NotFound();

            // Reverse balance impact
            transaction.Account.Balance += transaction.Type == TransactionType.Income
                                            ? -transaction.Amount
                                            : transaction.Amount;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return Ok(_mapper.Map<TransactionDetailsDto>(transaction));
        }
    }
}