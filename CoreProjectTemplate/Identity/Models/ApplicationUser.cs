using Microsoft.AspNetCore.Identity;
using Models.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Identity.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
    }
}
