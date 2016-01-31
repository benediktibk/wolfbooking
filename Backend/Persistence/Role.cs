using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Persistence
{
    public class Role
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
