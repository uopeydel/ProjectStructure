using Microsoft.EntityFrameworkCore;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Enums;
using Pjs1.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Pjs1.Common.GenericDbContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Pjs1.BLL.Implementations
{
    public class StaticFacade : IStaticFacade
    {
        private readonly IEntityFrameworkRepository<Interlocutor, MsSqlGenericDb> _interlocutorRepository;
        public StaticFacade(
             IEntityFrameworkRepository<Interlocutor, MsSqlGenericDb> interlocutorRepository
           )
        {
            _interlocutorRepository = interlocutorRepository;
        }

        public async Task<object> stFacade(int num)
        {
             _interlocutorRepository.GetAll().FirstOrDefaultAsync();
            throw new NotImplementedException();
        }
    }
     
}
