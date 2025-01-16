using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Groups = new HashSet<StudyGroup>();
            MaterialTags = new HashSet<MaterialTag>();
        }

        public int Idtag { get; set; }
        public string TagName { get; set; } = null!;

        public virtual ICollection<StudyGroup> Groups { get; set; }
        public virtual ICollection<MaterialTag> MaterialTags { get; set; }
    }
}
