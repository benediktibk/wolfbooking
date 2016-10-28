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
    public class User : IdentityUser
    {
        public User()
        { }

        [Required]
        public DateTime Deleted { get; set; } = DateTime.MaxValue;     
        public virtual Room Room { get; set; }

        public void UpdateWith(Business.User user)
        {
            // TODO update these lines
            //Login = user.Login;

            //if (!string.IsNullOrEmpty(user.Password))
            //    Password = user.Password;

            //Deleted = user.Deleted;
        }
    }
}
