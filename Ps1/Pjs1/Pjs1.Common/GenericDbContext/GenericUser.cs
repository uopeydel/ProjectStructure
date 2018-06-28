using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Pjs1.Common.Enums;

namespace Pjs1.Common.GenericDbContext
{
    public partial class GenericUser : IdentityUser<int>
    {
        public GenericUser()
        {

        }

        [Required] [StringLength(200)] public string FirstName { get; set; }
        [Required] [StringLength(200)] public string LastName { get; set; }
        public UserOnlineStatus OnlineStatus { get; set; }
        //public GenericUserRole UserRole { get; set; }
    }

    public class GenericUserLogin : IdentityUserLogin<int>
    {
    }

    public class GenericUserRole : IdentityUserRole<int>
    {
        public GenericUserRole()
        {
            //Roles = new HashSet<GenericRole>();
            //Users = new HashSet<GenericUser>();
        }



        //public ICollection<GenericUser> Users { get; set; }
        //public ICollection<GenericRole> Roles { get; set; }
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
        public GenericRole()
        {

        }

        public bool IsActive { get; set; }
        //public GenericUserRole UserRole { get; set; }
    }
}
