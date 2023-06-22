﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Website.Entity.Entities
{
    [Table("Specialized")]
    public class Specialized : BaseEntity<int>
    {
        [Required]
        public string Name { get; set; }
        public ICollection<Teacher> Teachers { get; }
    }
}
