using System.Security.Cryptography;
using System.Text;
using AspNetCoreAuthenAndAuthoWithJwt.Context;
using AspNetCoreAuthenAndAuthoWithJwt.Dtos;
using AspNetCoreAuthenAndAuthoWithJwt.Entities;
using AspNetCoreAuthenAndAuthoWithJwt.Services.Interfaces;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreAuthenAndAuthoWithJwt.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public AccountsController(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register([FromBody] RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
            {
                return BadRequest("User exists");
            }

            using var hmac = new HMACSHA512();
            var user = new User
            {
                UserName = registerDto.UserName,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            hmac.Dispose();

            return Ok(new UserDto()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            });
        }


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login([FromBody] LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null)
            {
                return NotFound("User not found");
            }

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computeHash.Length; i++)
            {
                if (computeHash[i] != user.PasswordHash[i])
                {
                    return Unauthorized("Invalid password");
                }
            }
            hmac.Dispose();

            return Ok(new UserDto()
            {
                UserName = user.UserName,
                Token = _tokenService.CreateToken(user),
            });
        }

        private async Task<bool> UserExists(string userName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == userName);
        }


    }
}
