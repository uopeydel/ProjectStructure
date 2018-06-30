using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pjs1.Common.GenericDbContext;

namespace Pjs1.Common.DAL.Models
{
    //คู่สนทนา
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
        public InterlocutorType InterlocutorType { get; set; }
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string StatusUnderName { get; set; }
        public string TimeZone { get; set; }

        public ICollection<Contact> ContactReceiver { get; set; }

        public ICollection<Contact> ContactSender { get; set; }

        public ICollection<Conversation> ConversationReceiver { get; set; }

        public ICollection<Conversation> ConversationSender { get; set; }

        [ForeignKey("UserId")]
        public GenericUser User { get; set; }
    }

    public enum InterlocutorType
    {
        None = 0,
        System = 1,
        User = 2
    }
}
