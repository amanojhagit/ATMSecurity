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

        [HttpGet]
        public IActionResult GetTransactions()
        {
            var transactions = _context.Transactions.ToList();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        public IActionResult GetTransaction(Guid id)
        {
            var transaction = _context.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        [HttpPost]
        public IActionResult CreateTransaction([FromBody] Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            return Ok(transaction);
        }

        [HttpPut("{id}")]
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

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] Transaction transaction)
        {
            if (transaction.TransactionType != "Deposit")
            {
                return BadRequest("Invalid transaction type");
            }

            if (transaction.TransactionAmount <= 0)
            {
                return BadRequest("Invalid amount");
            }


            _context.Transactions.Add(transaction);
            _context.SaveChanges();

            return Ok(transaction);
        }

    }
}
