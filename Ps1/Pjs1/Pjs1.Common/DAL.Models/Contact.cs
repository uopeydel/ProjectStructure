using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    //รายชื่อผู้ติดต่อ รายชื่อเพื่อน
    public partial class Contact
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ContactId { get; set; }
        public ContactStatus SenderStatus { get; set; }
        public ContactStatus ReceiverStatus { get; set; }
        public DateTimeOffset ActionTime { get; set; }//??


        public int ContactReceiverId { get; set; }
        public Interlocutor ContactReceiver { get; set; }

        public int ContactSenderId { get; set; }
        public Interlocutor ContactSender { get; set; }
    }

    public enum ContactStatus
    {
        None = 0,
        Sended = 1, //คนส่งคำขอเป็นผู้ติดต่อ กดส่งแล้ว
        Received = 2, //คนได้รับคำขอติดต่อ ได้รับแล้ว
        Contacted = 3, //เห็นว่าอีกฝ่ายเป็นผู้สนทนาอยู่  สามารถส่งข้อความไปหาอีกฝ่ายได้
        Hided = 4, //ซ่อนการส่งข้อความทุกชนิดจากอีกฝ่าย และจะไม่มีการแจ้งเตือนจากเซิฟเวอรขึ้นไป อีกฝ่ายจะขึ้นเป็น  IsContact = 3 ส่งข้อความมาได้ แต่เราจะไม่เห็น เพราะเรากดซ่อนไว้
        Blocked = 5, //บลอคการติดต่อ อีกฝ่ายจะไม่เห็นบัญชีเราในรายชื่อผู้ติดต่อ
        Deleted = 6 //ถูกลบ สถานะจะเหมือน None = 0 สามารถส่งคำร้องขอใหม่ได้เพื่อติดต่อ

    }
}
