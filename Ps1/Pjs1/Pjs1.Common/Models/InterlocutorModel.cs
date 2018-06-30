using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.GenericDbContext;

namespace Pjs1.Common.Models
{
    public partial class InterlocutorModel
    {
        public int InterlocutorId { get; set; }
        public InterlocutorType InterlocutorType { get; set; } 
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string StatusUnderName { get; set; }
        public string TimeZone { get; set; } 
        public List<Contact> Contacts { get; set; }
        public List<Conversation> Conversations { get; set; }
    }

}
