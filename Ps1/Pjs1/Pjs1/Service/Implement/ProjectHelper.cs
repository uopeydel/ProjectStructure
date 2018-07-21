using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjs1.BLL.Interfaces;
using Pjs1.Common.Models;
using Pjs1.Main.Service.Interface;

namespace Pjs1.Main.Service.Implement
{
    public class ProjectHelper : IProjectHelper
    {
        private readonly IChatService _iChatService;

        public ProjectHelper(IChatService iChatService)
        {
            _iChatService = iChatService;
        }

        public async Task<InterlocutorModel> GetInterlocutorWithContact(int interlocutorId)
        {
            return await _iChatService.GetInterlocutorWithContact(interlocutorId);
        }
    }
}
