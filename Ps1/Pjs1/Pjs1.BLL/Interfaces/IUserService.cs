using Pjs1.Common.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pjs1.Common.Enums;
using Pjs1.Common.GenericDbContext;

namespace Pjs1.BLL.Interfaces
{
    public interface IUserService
    {
        Task TestGenericEf();
        Task<GenericUser> AddUser(GenericUser user);
        Task<GenericUser> UpdateUserSomeProperties(GenericUser user);
        Task<GenericUser> UpdateUserSomePropertiesWork(GenericUser user);
        Task<List<GenericUser>> GetUserAll();


        Task<GenericUser> UpdateOnlineStatus(UserOnlineStatus onlneStatus);
        Task<GenericUser> UpdateOnlineStatusMultiType(UserOnlineStatus onlneStatus, string lastName);

        
    }
}
