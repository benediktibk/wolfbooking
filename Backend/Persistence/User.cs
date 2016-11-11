using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Backend.Persistence
{
    public class User : IdentityUser<int, WolfBookingUserLogin, WolfBookingUserRole, WolfBookingUserClaim>
    {
        public User()
        { }

        [Required]
        public DateTime Deleted { get; set; } = DateTime.MaxValue;     
        public virtual Room Room { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(WolfBookingUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }
}
