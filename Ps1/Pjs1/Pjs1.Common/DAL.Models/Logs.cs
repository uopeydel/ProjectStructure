using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pjs1.Common.DAL.Models
{
    public partial class Logs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LogId { get; set; }
        public string TableName { get; set; }
        public int PkId { get; set; }
        public string PreviousData { get; set; }
        public string CurrentData { get; set; }
        public DateTimeOffset LogDatetime { get; set; }
    }
}
