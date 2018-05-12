using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    public partial class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ContactId { get; set; }
        public int SenderStatus { get; set; }
        public int ReceiverStatus { get; set; }
        public DateTimeOffset ActionTime { get; set; }


        public int ContactReceiverId { get; set; }
        public Interlocutor ContactReceiver { get; set; }

        public int ContactSenderId { get; set; }
        public Interlocutor ContactSender { get; set; }
    }
}
