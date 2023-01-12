using ATMDemo.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;
using Transaction = ATMDemo.Models.Transaction;

namespace ATMDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionController(AppDbContext context)
        {
            _context = context;
        }

        //GET/transactions
        [HttpGet]
        public IActionResult GetTransactions()
        {
            try
            {
                var transactions = _context.Transactions.ToList();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //GET/transactions/{id}
        [HttpGet("{id}")]
        public IActionResult GetTransaction(Guid id)
        {
            try
            {
                var transaction = _context.Transactions.Find(id);
                if (transaction == null)
                {
                    return NotFound();
                }

                return Ok(transaction);
            }
            catch (Exception)
            {
                // Log the exception
                return StatusCode(500, "An error occurred while trying to retrieve the transaction. Please try again later.");
            }
        }


        //POST/transactions
        /*[HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            try
            {
                _context.Transactions.Add(transaction);
                _context.SaveChanges();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest("Transaction creation failed" + ex.GetType().Name);
            }
        }
*/
        //PUT/transactions/{id}
        /*[HttpPut("{id}")]
        public IActionResult UpdateTransaction(Guid id, [FromBody] Transaction transaction)
        {
            var existingTransaction = _context.Transactions.Find(id);
            if (existingTransaction == null)
            {
                return NotFound();
            }

            existingTransaction.AccountNo = transaction.AccountNo;
            existingTransaction.TransactionAmount = transaction.TransactionAmount;
            existingTransaction.Purpose = transaction.Purpose;
            existingTransaction.Date = transaction.Date;
            existingTransaction.TransactionType = transaction.TransactionType;

            _context.Transactions.Update(existingTransaction);
            _context.SaveChanges();
            return Ok(existingTransaction);
        }


        //DELETE/transactions/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteTransaction(Guid id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return NoContent();
        }
*/
        //POST/transactions/deposit
        [HttpPost("deposit")]
        public IActionResult Deposit(int accountNo, decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }

            //check whether the accountNo is present in Account or not 
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNo == accountNo);
            if (account == null)
            {
                return NotFound("Account not found");
            }

            var transaction = new Transaction
            {
                AccountNo = accountNo,
                Date = DateTime.Now,
                Purpose = "Deposit",
                TransactionAmount = amount,
                TransactionType = "Deposit"
            };

            try
            {
                _context.Transactions.Add(transaction);
                account.Amount += amount;
                _context.SaveChanges();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest("Withdraw failed" + ex.GetType().Name);
            }
        }

        //POST/transactions/withdraw
        [HttpPost("withdraw")]
        public IActionResult Withdraw(int accountNo, decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }

            //check whether the accountNo is present in Account or not 
            var account = _context.Accounts.FirstOrDefault(a => a.AccountNo == accountNo);
            if (account == null)
            {
                return NotFound("Account not found");
            }
            //Checking if account balance is sufficient
            if (amount > account.Amount)
            {
                return BadRequest("Insufficient balance");
            }

            var transaction = new Transaction
            {
                AccountNo = accountNo,
                Date = DateTime.Now,
                Purpose = "Withdraw",
                TransactionAmount = amount,
                TransactionType = "Withdraw"
            };

            try
            {
                _context.Transactions.Add(transaction);
                account.Amount -= amount;
                _context.SaveChanges();
                return Ok(transaction);
            }
            catch (Exception ex)
            {
                return BadRequest("Withdraw failed" + ex.GetType().Name);
            }
        }


        [HttpPost("transfer")]
        public IActionResult Transfer(int senderAccountNo, int receiverAccountNo, decimal amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }

            var senderAccount = _context.Accounts.FirstOrDefault(a => a.AccountNo == senderAccountNo);
            var receiverAccount = _context.Accounts.FirstOrDefault(a => a.AccountNo == receiverAccountNo);
            if (senderAccount == null || receiverAccount == null)
            {
                return NotFound("Account not found");
            }
            if (amount > senderAccount.Amount)
            {
                return BadRequest("Insufficient balance");
            }

            var withdrawalTransaction = new Transaction
            {
                AccountNo = senderAccountNo,
                Date = DateTime.Now,
                Purpose = "Withdraw",
                TransactionAmount = amount,
                TransactionType = "Withdraw"
            };
            var depositTransaction = new Transaction
            {
                AccountNo = receiverAccountNo,
                Date = DateTime.Now,
                Purpose = "Deposit",
                TransactionAmount = amount,
                TransactionType = "Deposit"
            };
            _context.Transactions.Add(withdrawalTransaction);
            _context.Transactions.Add(depositTransaction);

            senderAccount.Amount -= amount;
            receiverAccount.Amount += amount;
            try
            {
                _context.SaveChanges(acceptAllChangesOnSuccess: true);
                return Ok("Transfer success");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest("Transfer failed" + ex.GetType().Name);
            }
        }

    }
}
