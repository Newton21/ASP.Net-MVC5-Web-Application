using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        [Display(Name="Semester Name")]
        public string SemesterName { get; set; }

        public virtual List<Course> CourseList { get; set; }
    }
}