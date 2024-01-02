using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ledger.Ledger.Web.Models;
using Ledger.Ledger.Web.Repositories;
using Ledger.Ledger.Web.UnitOfWork;
namespace Ledger.Ledger.Web.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserAsync(int id, User newUser);
        Task<User> DeleteUserAsync(int id);
    }

    public class UserService :IUserService// bussiness logic layer, uses repository layer
    {
        private readonly IUserRepository _userRepository;
        private IUnitOfWork _unitOfWork;
        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);

        }

        public async Task<User> AddUserAsync(User user)
        {
            try
            { 
                await _userRepository.AddUserAsync(user);
                await _unitOfWork.CommitAsync();
                return user;

            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(int id, User newUser)
        {
            try
            {
                var user = await _userRepository.UpdateUserAsync(id, newUser);
                await _unitOfWork.CommitAsync();
                return user;

            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task<User> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _userRepository.DeleteUserAsync(id);
                await _unitOfWork.CommitAsync();
                return user;

            }
            catch (Exception e)
            {
                await _unitOfWork.RollBackAsync();
                Console.WriteLine(e);
                throw;
            }
        }
    }
}