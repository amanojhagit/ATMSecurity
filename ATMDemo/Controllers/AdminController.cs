using ATMDemo.Data;
using ATMDemo.Models;
using Microsoft.AspNetCore.Mvc;

namespace ATMDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Admin>> GetUsers()
        {
            return _context.User.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Admin> GetUser(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        [HttpPost]
        public ActionResult<Admin> CreateUser(Admin user)
        {
            _context.User.Add(user);
            _context.SaveChanges();

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public ActionResult<Admin> UpdateUser(int id, Admin user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }
            //_context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.User.Update(user);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult<Admin> DeleteUser(int id)
        {
            var user = _context.User.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            _context.SaveChanges();

            return user;
        }
    }

}
