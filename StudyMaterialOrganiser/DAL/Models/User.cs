using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class User
    {
        public User()
        {
            MaterialUsers = new HashSet<MaterialUser>();
            UserGroups = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string Username { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PwdHash { get; set; } = null!;
        public string PwdSalt { get; set; } = null!;
        public string? Phone { get; set; }
        public int Role { get; set; }
        public string SecurityToken { get; set; } = null!;
        public bool IsDeleted { get; set; }

        public virtual ICollection<MaterialUser> MaterialUsers { get; set; }
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
