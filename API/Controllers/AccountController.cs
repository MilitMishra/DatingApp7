
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.IServices;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext _context { get; }
        public ITokenService _tokenService { get; }
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _tokenService = tokenService;
             _context = context;
            
        }

        [HttpPost("register")] //api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDTo registerDTo){
            if(await IsUserExist(registerDTo.Username)) return BadRequest("Username is taken!");
            using var hmac=new HMACSHA512();
            var user = new AppUser{
                UserName=registerDTo.Username.ToLower(),
                PassswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTo.Password)),
                PassswordSalt=hmac.Key
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return new UserDto(){
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDTo loginDTo){
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName==loginDTo.Username.ToLower());

            if(user == null) return Unauthorized("Username not valid");

            using var hmac = new HMACSHA512(user.PassswordSalt);
            var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTo.Password));

            for(int i=0; i<passwordHash.Length; i++){
                if(passwordHash[i]!=user.PassswordHash[i])
                return Unauthorized("invalid password");
            }

            return new UserDto(){
                Username=user.UserName,
                Token=_tokenService.CreateToken(user)
            };

        }

        public async Task<bool> IsUserExist(string Username){
            return await _context.Users.AnyAsync(x=>x.UserName==Username.ToLower());
        } 
    }
}