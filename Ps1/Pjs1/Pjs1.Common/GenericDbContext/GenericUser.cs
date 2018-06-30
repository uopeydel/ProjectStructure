using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Pjs1.Common.DAL.Models;
using Pjs1.Common.Enums;

namespace Pjs1.Common.GenericDbContext
{
    public partial class GenericUser : IdentityUser<int>
    {
        [Required] [StringLength(200)] public string FirstName { get; set; }
        [Required] [StringLength(200)] public string LastName { get; set; }
        public UserOnlineStatus OnlineStatus { get; set; }

        public int InterlocutorId { get; set; }
        [ForeignKey("InterlocutorId")]
        public Interlocutor Interlocutor { get; set; }

    }

    public class GenericUserLogin : IdentityUserLogin<int>
    {
    }

    public class GenericUserRole : IdentityUserRole<int>
    {
    }

    public class GenericUserClaim : IdentityUserClaim<int>
    {
    }

    public class GenericRoleClaim : IdentityRoleClaim<int>
    {
    }

    public class GenericUserToken : IdentityUserToken<int>
    {
    }

    public class GenericRole : IdentityRole<int>
    {

        public bool IsActive { get; set; }
    }
}
