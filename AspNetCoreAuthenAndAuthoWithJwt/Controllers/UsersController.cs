using AspNetCoreAuthenAndAuthoWithJwt.Context;
using AspNetCoreAuthenAndAuthoWithJwt.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreAuthenAndAuthoWithJwt.Controllers
{
    public class UsersController : BaseApiController
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("all")]
        public async Task<ActionResult<User>> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUsers(int id)
        {
            return Ok(await _context.Users.FindAsync(id));
        }
    }
}
