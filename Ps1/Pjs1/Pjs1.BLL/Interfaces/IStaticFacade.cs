using Pjs1.Common.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pjs1.Common.Enums;
using Pjs1.Common.GenericDbContext;

namespace Pjs1.BLL.Interfaces
{
    public interface IStaticFacade
    {
        Task<object> stFacade(int num);
       
    }
}
