using ATMDemo.Data;
using ATMDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ATMDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            var accounts = _context.Accounts.ToList();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(Guid id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            return Ok(account);
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
            return Ok(account);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateAccount(Guid id, [FromBody] Account account)
        {
            var existingAccount = _context.Accounts.Find(id);
            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.AccountNo = account.AccountNo;
            existingAccount.AccountName = account.AccountName;
            existingAccount.Amount = account.Amount;
            existingAccount.Address = account.Address;
            existingAccount.PhoneNo = account.PhoneNo;
            existingAccount.Passport = account.Passport;
            existingAccount.DateOfOpened = account.DateOfOpened;
            existingAccount.AccountType = account.AccountType;
            existingAccount.FingerprintID = account.FingerprintID;
            existingAccount.SMS = account.SMS;
            existingAccount.SMSport = account.SMSport;

            _context.Accounts.Update(existingAccount);
            _context.SaveChanges();
            return Ok(existingAccount);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(Guid id)
        {
            var account = _context.Accounts.Find(id);
            if (account == null)
            {
                return NotFound();
            }

            _context.Accounts.Remove(account);
            _context.SaveChanges();
            return NoContent();
        }
    }

}
