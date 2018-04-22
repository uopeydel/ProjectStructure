using Microsoft.EntityFrameworkCore;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;
using Pjs1.DAL.PostgreSQL.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pjs1.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IEntityFrameworkRepository<User, Postgre1DbContext> _userRepository;
        public UserService(IEntityFrameworkRepository<User, Postgre1DbContext> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddUser(User user)
        {
            var usr = await _userRepository.AddAsync(user);
            _userRepository.SaveChanges();
            return usr;
        }

        public async Task<List<User>> GetUserAll()
        {
            var usr = await _userRepository.GetAll().ToListAsync();
            return usr;
        }
    }
}
