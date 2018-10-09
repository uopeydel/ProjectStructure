using Microsoft.EntityFrameworkCore;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Enums;
using Pjs1.Common.GenericDbContext;
using Pjs1.DAL.Implementations;
using Pjs1.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Pjs1.BLL.Implementations
{
    public class UserService : IUserService
    {
        private readonly IEntityFrameworkRepository<GenericUser, MsSqlGenericDb> _userRepository;
        private readonly IGenericEFRepository _genericEf;
        public UserService(IGenericEFRepository genericEf/*, IEntityFrameworkRepository<GenericUser, MsSqlGenericDb> userRepository*/)
        {
            _genericEf = genericEf;
            //_userRepository = userRepository;
        }

        public async Task TestGenericEf() {
           
            await _genericEf.TestGet();
        }

        public async Task<GenericUser> AddUser(GenericUser user)
        {

            var usr = await _userRepository.AddAsync(user);
            _userRepository.SaveChanges();
            return usr;
        }

        public async Task<List<GenericUser>> GetUserAll()
        {
            var usr = await _userRepository.GetAll().ToListAsync();
            return usr;
        }

        public async Task<GenericUser> UpdateOnlineStatus(UserOnlineStatus onlneStatus)
        {
            await Task.Run(() =>
            {
                _userRepository
                .UpdateSpecficProperty(

                 new GenericUser { Id = 1, OnlineStatus = onlneStatus },

                  o => o.OnlineStatus
                        );
                _userRepository.SaveChanges();
            });
            var usr = await _userRepository.GetAll(a => a.Id == 1, false).FirstAsync();
            return usr;
        }

        public async Task<GenericUser> UpdateOnlineStatusMultiType(UserOnlineStatus onlneStatus, string lastName)
        {
            await Task.Run(() =>
            {
                _userRepository
                    .UpdateSpecficPropertyMultiType(

                        new GenericUser { Id = 1, OnlineStatus = onlneStatus, LastName = lastName },

                        o => o.OnlineStatus, o => o.LastName
                    );
                _userRepository.SaveChanges();
            });
            var usr = await _userRepository.GetAll(a => a.Id == 1, false).FirstAsync();
            return usr;
        }

        public async Task<GenericUser> UpdateUserSomeProperties(GenericUser user)
        {
            user = new GenericUser
            {
                Email = "test1@email.com",
                Id = 1,
                FirstName = "firstname" + Guid.NewGuid(),
                LastName = "lastname" + DateTime.Now.ToString("o"),
                UserName = "uname1"
            };

            await Task.Run(() =>
            {
                _userRepository.UpdateSpecficProperty(user, new Expression<Func<GenericUser, object>>[] { });
                _userRepository.SaveChanges();
            });

            var usr1 = await _userRepository.GetAll(g => g.Id == 1).FirstOrDefaultAsync();
            await Task.Run(() =>
            {
                usr1.FirstName = "first";
                usr1.LastName = "last";
                usr1.Email = "xxx";
                usr1.UserName = "xxx";

                _userRepository.UpdateSpecficProperty(usr1, o => o.LastName, o => o.FirstName);
                _userRepository.SaveChanges();
            });
            var usr2 = await _userRepository.GetAll(g => g.Id == 1).FirstOrDefaultAsync();
            return usr2;
        }

        public async Task<GenericUser> UpdateUserSomePropertiesWork(GenericUser user)
        {
            if (user == null)
            {
                user = new GenericUser
                {
                    Id = 1,
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
            var usrResult = await _userRepository.GetAll(g => g.Id == 1).FirstOrDefaultAsync();
            return usrResult;
        }




    }
}
