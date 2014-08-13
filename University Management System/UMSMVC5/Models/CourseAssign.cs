using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class CourseAssign
    {
        [Key]
        public int CourseAssignId { get; set; }

        public int CourseId { get; set; }

        public int TeacherId { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course  { get; set; }

        [ForeignKey("TeacherId")]
        public virtual Teacher Teacher { get; set; }
    }
}