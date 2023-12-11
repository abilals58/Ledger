using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Data;
using Ledger.Ledger.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Ledger.Ledger.Web.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User newUser);
        Task<User> DeleteUserAsync(int id);
    }
    
    public class UserRepository :IUserRepository // User service corresponds to data access tier and handles database operations
    {
        private readonly IDbContext _dbContext;

        public UserRepository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task<IEnumerable<User>> GetAllUsersAsync() // returns all users 
        {
            return await _dbContext.Users.ToListAsync();
        }
        
        public async Task<User> GetUserByIdAsync(int id) // returns a user by id, return null if there is no match
        {
            return await _dbContext.Users.FindAsync(id);
        }
        
        public async Task<User> AddUserAsync(User user) // ads new user 
        {
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }
        
        public async Task<User> UpdateUserAsync(int id, User newUser) // updates a user and return that user, return null if there is no match
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;
            user.Name = newUser.Name;
            user.Surname = newUser.Surname;
            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            user.Password = newUser.Password;
            user.Phone = newUser.Phone;
            await _dbContext.SaveChangesAsync();
            return user;
        }
        
        public async Task<User> DeleteUserAsync(int id) // deletes a user, return null if there is no match
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null) return null;
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }
    }
}