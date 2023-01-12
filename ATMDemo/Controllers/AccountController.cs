using ATMDemo.Controllers.Dto;
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

        //GET /accounts: retrieve a list of accounts
        [HttpGet]
        public IActionResult GetAccounts()
        {
            try
            {
                var accounts = _context.Accounts.ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //GET /accounts/{id}: retrieve a specific account by ID
        [HttpGet("{id}")]
        public IActionResult GetAccount(Guid id)
        {
            try
            {
                var account = _context.Accounts.Find(id);
                if (account == null)
                {
                    return NotFound();
                }

                return Ok(account);
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while trying to retrieve an account. Please try again later.");
            }
        }

        //POST /accounts: create a new account
        [HttpPost]
        public IActionResult CreateAccount([FromBody] AccountCreateDto accountCreateDto)
        {
            try
            {
                var account = new Account
                {
                    AccountName = accountCreateDto.AccountName,
                    Amount = accountCreateDto.Amount,
                    Address = accountCreateDto.Address,
                    PhoneNo = accountCreateDto.PhoneNo,
                    Passport = accountCreateDto.Passport,
                    AccountType = accountCreateDto.AccountType,
                    FingerprintID = accountCreateDto.FingerprintID,
                    SMS = accountCreateDto.SMS,
                    SMSport = accountCreateDto.SMSport,
                };
                _context.Accounts.Add(account);
                _context.SaveChanges();
                return Ok(account);
            }
            catch (Exception ex)
            {
                return BadRequest("Error creating account: " + ex.Message);
            }
        }

        //PUT /accounts/{id}: update an existing account
        [HttpPut("{id}")]
        public IActionResult UpdateAccount(Guid id, [FromBody] AccountCreateDto accountCreateDto)
        {
            try
            {
                var existingAccount = _context.Accounts.Find(id);
                if (existingAccount == null)
                {
                    return NotFound();
                }
                existingAccount.AccountName = accountCreateDto.AccountName;
                existingAccount.Amount = accountCreateDto.Amount;
                existingAccount.Address = accountCreateDto.Address;
                existingAccount.PhoneNo = accountCreateDto.PhoneNo;
                existingAccount.Passport = accountCreateDto.Passport;
                existingAccount.AccountType = accountCreateDto.AccountType;
                existingAccount.FingerprintID = accountCreateDto.FingerprintID;
                existingAccount.SMS = accountCreateDto.SMS;
                existingAccount.SMSport = accountCreateDto.SMSport;

                _context.Accounts.Update(existingAccount);
                _context.SaveChanges();
                return Ok(existingAccount);
            }
            catch (Exception ex)
            {
                return BadRequest("Failed to update account. Error: " + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(Guid id)
        {
            try
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
            catch(Exception ex)
            {
                return BadRequest("DeleteAccount failed" + ex.GetType().Name);
            }

        }
    }

}
