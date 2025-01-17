using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class StudyGroup
    {
        public StudyGroup()
        {
            UserGroups = new HashSet<UserGroup>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int TagId { get; set; }

        public virtual Tag Tag { get; set; } = null!;
        public virtual ICollection<UserGroup> UserGroups { get; set; }
    }
}
