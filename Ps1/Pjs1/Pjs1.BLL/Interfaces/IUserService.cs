using Pjs1.Common.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pjs1.BLL.Interfaces
{
    public interface IUserService
    {
        Task<User> AddUser(User user);
        Task<User> UpdateUserSomeProperties(User user);
        Task<User> UpdateUserSomePropertiesWork(User user);
        Task<List<User>> GetUserAll();
    }
}
