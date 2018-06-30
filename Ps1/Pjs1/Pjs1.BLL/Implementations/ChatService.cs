using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.GenericDbContext;
using Pjs1.Common.Models;
using Pjs1.DAL.Interfaces;

namespace Pjs1.BLL.Implementations
{
    public class ChatService : IChatService
    {
        private readonly IEntityFrameworkRepository<Contact, MsSqlGenericDb> _contactRepository;
        private readonly IEntityFrameworkRepository<Conversation, MsSqlGenericDb> _conversationRepository;
        private readonly IEntityFrameworkRepository<Interlocutor, MsSqlGenericDb> _interlocutorRepository;

        public ChatService(
            IEntityFrameworkRepository<Contact, MsSqlGenericDb> contactRepository,
            IEntityFrameworkRepository<Conversation, MsSqlGenericDb> conversationRepository,
            IEntityFrameworkRepository<Interlocutor, MsSqlGenericDb> interlocutorRepository
            )
        {
            _contactRepository = contactRepository;
            _conversationRepository = conversationRepository;
            _interlocutorRepository = interlocutorRepository;
        }

        public async Task AddContact(Contact contact)
        {
            await _contactRepository.AddAsync(contact);
            _contactRepository.SaveChanges();
        }

        public async Task CreateInterlocutor(Interlocutor interlocutor)
        {
            await _interlocutorRepository.AddAsync(interlocutor);
            _interlocutorRepository.SaveChanges();
        }

        //https://localhost:44372/api/chat/GetInterlocutorWithContact/1
        public async Task<InterlocutorModel> GetInterlocutorWithContact(int interlocutorId)
        {
            try
            {
                var result = await _interlocutorRepository
                    .GetAll(g => g.InterlocutorId == interlocutorId) 
                    .Select(s => new InterlocutorModel
                    {
                        InterlocutorType = s.InterlocutorType,
                        InterlocutorId = s.InterlocutorId,
                        DisplayName = s.DisplayName,
                        ProfileImageUrl = s.ProfileImageUrl,
                        StatusUnderName = s.StatusUnderName,
                        TimeZone = s.TimeZone,
                       // Contacts = s.Contacts.Where(w => w.ContactReceiverId == interlocutorId || w.se)
                    })
                    .FirstOrDefaultAsync() ;
                return result;
            }
            catch (Exception e)
            { 
                throw e;
            }
          
        }

    }
}
