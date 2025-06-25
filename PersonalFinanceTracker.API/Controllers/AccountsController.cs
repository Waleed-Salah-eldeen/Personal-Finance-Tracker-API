using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalFinanceTracker.API.Dtos.AccountDtos;
using System.Collections.Generic;
namespace PersonalFinanceTracker.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public AccountsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDetailsDto>>> GetAccounts()
        {
            var accounts = await _context.Accounts.OrderBy(a => a.CreatedAt).AsNoTracking().ToListAsync();

            var accountsDetailsDto = _mapper.Map<IEnumerable<AccountDetailsDto>>(accounts);

            return Ok(accountsDetailsDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDetailsDto>> GetAccountById(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
                return NotFound();

            return Ok(_mapper.Map<AccountDetailsDto>(account));
        }

        [Route("[Action]/{name}")]
        [HttpGet]
        public async Task<ActionResult<AccountDetailsDto>> GetAccountByName(string name)
        {
            var account = await _context.Accounts.SingleOrDefaultAsync(a => a.Name == name);
            if (account is null)
                return NotFound();

            return Ok(_mapper.Map<AccountDetailsDto>(account));
        }

        [HttpPost]
        public async Task<ActionResult<AccountDetailsDto>> CreateAccount([FromBody] AccountDto dto)
        {
            var isNotValidAccountName = _context.Accounts.Any(a => a.Name == dto.Name);
            if (isNotValidAccountName)
                return BadRequest($"You allready have an account with the \"{dto.Name}\" name !");
            if (dto.Balance <= 0)
                return BadRequest("enter a valid Balance");
            var account = _mapper.Map<Account>(dto);
            account.CreatedAt = DateTime.Now;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            var accountDetailsDto = _mapper.Map<AccountDetailsDto>(account);
            return Ok(accountDetailsDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] AccountDto accountDto)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account == null)
                return NotFound();

            _mapper.Map(accountDto, account);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<AccountDetailsDto>(account));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<AccountDetailsDto>> DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id);
            if (account is null)
                return NotFound();
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();

            return Ok(_mapper.Map<AccountDetailsDto>(account));
        }
    }
}