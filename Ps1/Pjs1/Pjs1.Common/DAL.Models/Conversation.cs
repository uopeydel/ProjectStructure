using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    public partial class Conversation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ConversationId { get; set; }

        public string Message { get; set; }
        public DateTimeOffset SendTime { get; set; }
        public int MessageDataType { get; set; }



        public int ConversationReceiverId { get; set; }
        public Interlocutor ConversationReceiver { get; set; }

        public int ConversationSenderId { get; set; }
        public Interlocutor ConversationSender { get; set; }
    }
}
