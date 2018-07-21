using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pjs1.Common.Models;

namespace Pjs1.Main.Service.Interface
{
    public   interface IProjectHelper
    {
        Task<InterlocutorModel> GetInterlocutorWithContact(int interlocutorId);
    }
}
