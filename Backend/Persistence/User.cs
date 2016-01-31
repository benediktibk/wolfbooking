using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Persistence
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Index(IsUnique = true)]
        [StringLength(LoginMaximumLength)]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public DateTime Deleted { get; set; }

        public virtual ICollection<Role> Roles { get; set; }

        public const int LoginMaximumLength = 100;
    }
}
