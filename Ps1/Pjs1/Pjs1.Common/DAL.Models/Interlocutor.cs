using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    public partial class Interlocutor
    {
        public Interlocutor()
        {
            ContactReceiver = new HashSet<Contact>();
            ContactSender = new HashSet<Contact>();
            ConversationReceiver = new HashSet<Conversation>();
            ConversationSender = new HashSet<Conversation>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int InterlocutorId { get; set; }
        public int InterlocutorType { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string ProfileImageUrl { get; set; }
        public string StatusUnderName { get; set; }
        public string TimeZone { get; set; }

        public ICollection<Contact> ContactReceiver { get; set; }

        public ICollection<Contact> ContactSender { get; set; }

        public ICollection<Conversation> ConversationReceiver { get; set; }

        public ICollection<Conversation> ConversationSender { get; set; }
    }
}
