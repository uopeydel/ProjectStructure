using Microsoft.EntityFrameworkCore;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;
using Pjs1.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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

        public async Task<User> UpdateUserSomeProperties(User user)
        { 
            user = new User
            {
                Email = "test1@email.com",
                UserId = 1,
                FirstName = "firstname" + Guid.NewGuid(),
                LastName = "lastname" + DateTime.Now.ToString("o"),
                UserName = "uname1"
            };

            await Task.Run(() =>
            {
                _userRepository.UpdateSpecficProperty(user, new Expression<Func<User, object>>[] { });
                _userRepository.SaveChanges();
            });
            var usr1 = await _userRepository.GetAll(g => g.UserId == 1).FirstOrDefaultAsync();

            await Task.Run(() =>
            {
                var userUpdateNew = new User
                {
                    FirstName = "first",
                    LastName = "last",
                    Email = "xxx",
                    UserName = "xxx",
                };
                _userRepository.UpdateSpecficProperty(userUpdateNew, o => o.LastName, o => o.FirstName);
                _userRepository.SaveChanges();
            });
            var usr2 = await _userRepository.GetAll(g => g.UserId == 1).FirstOrDefaultAsync();
            return usr2;
        }

        public async Task<User> UpdateUserSomePropertiesWork(User user)
        {
            if (user == null)
            {
                user = new User
                {
                    UserId = 1,
                    FirstName = "firstname" + Guid.NewGuid(),
                    LastName = "lastname" + DateTime.Now.ToString("o"),
                    Email = "xxx",
                    UserName = "xxx"
                };
            }
            await Task.Run(() =>
            {
                _userRepository.UpdateSpecficProperty(user, o => o.LastName, o => o.FirstName);
                _userRepository.SaveChanges();
            });
            var usrResult = await _userRepository.GetAll(g => g.UserId == 1).FirstOrDefaultAsync();
            return usrResult;
        }
    }
}
