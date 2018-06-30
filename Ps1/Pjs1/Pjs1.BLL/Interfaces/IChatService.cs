using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Models;

namespace Pjs1.BLL.Interfaces
{
    public interface IChatService
    {
        Task CreateInterlocutor(Interlocutor interlocutor);
        Task AddContact(Contact contact);
        Task<InterlocutorModel> GetInterlocutorWithContact(int interlocutorId);
    }
}
