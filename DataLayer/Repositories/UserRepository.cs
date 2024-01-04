using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AccountingDbContext _context;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        public UserRepository(AccountingDbContext context,IPasswordHasher<ApplicationUser> passwordHasher)
        {

            _context = context;
            _passwordHasher = passwordHasher;

        }

        public async Task<string> AddSecretCode(string name)
        {
            string NormalizerName = name.ToUpper();
            var user = await _context.ApplicationUsers.SingleAsync(x=>x.NormalizedEmail == NormalizerName);
            string code = GenerateCode();
            user.SecurityStamp = code;
            _context.SaveChangesAsync();
            return code;
        }

        private string GenerateCode()
        {
            var rnd = new Random((int)DateTime.Now.Ticks);
            return rnd.Next(10000, 99999).ToString();
        }

        public async Task<bool> UserExist(string name)
        {
            string NormalizerName = name.ToUpper();
            return await _context.ApplicationUsers.AnyAsync(x=>x.NormalizedUserName == NormalizerName);
        }

        public async Task<bool> VerifiCode(string name, string code)
        {
            string NormalizerName = name.ToUpper();
            var user = await _context.ApplicationUsers.SingleAsync(x => x.NormalizedEmail == NormalizerName);
            return user.SecurityStamp == code;
        }

        public async void DeleteCode(string name)
        {
            string NormalizerName = name.ToUpper();
            var user = await _context.ApplicationUsers.SingleAsync(x => x.NormalizedEmail == NormalizerName);
            user.SecurityStamp = null;
            _context.SaveChangesAsync();
        }

        public async void ResetPassword(string name, string password)
        {
            string NormalizerName = name.ToUpper();
            var user = await _context.ApplicationUsers.SingleAsync(x => x.NormalizedEmail == NormalizerName);
            user.PasswordHash = _passwordHasher.HashPassword(user,password);
            _context.SaveChangesAsync();
            
        }

        public async void DeleteUser()
        {
            string NormalizerName = UserContext.UserName.ToUpper();
            var user = await _context.ApplicationUsers.SingleAsync(x => x.NormalizedEmail == NormalizerName);
            _context.ApplicationUsers.Remove(user);
            

        }
    }
}
