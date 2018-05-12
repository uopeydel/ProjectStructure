using Pjs1.Common.DAL.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Pjs1.Common.DAL.Models
{
    public class User : BaseOnUser
    {
        [Required] [StringLength(200)] public string Email { get; set; }
        [Required] [StringLength(20)] public string UserName { get; set; }
        [Required] [StringLength(200)] public string FirstName { get; set; }
        [Required] [StringLength(200)] public string LastName { get; set; }
    }
}
