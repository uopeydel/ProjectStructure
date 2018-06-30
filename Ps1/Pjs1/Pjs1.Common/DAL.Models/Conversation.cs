using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    //การสนทนา
    public partial class Conversation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ConversationId { get; set; }

        public string Message { get; set; }
        public DateTimeOffset SendTime { get; set; }
        public MessageDataType MessageDataType { get; set; }



        public int ConversationReceiverId { get; set; }
        public Interlocutor ConversationReceiver { get; set; }

        public int ConversationSenderId { get; set; }
        public Interlocutor ConversationSender { get; set; }
    }

    public enum MessageDataType
    {
        None = 0,
        Text = 1,
        Image = 2,
        Video = 3,
        Audio = 4,
        Url = 5,
        File = 6,
        Sticker
    }

    public enum ConversationStatus
    {
        None = 0,
        Unread = 1,
        Readed = 2,
        Deleted = 3

    }
}
