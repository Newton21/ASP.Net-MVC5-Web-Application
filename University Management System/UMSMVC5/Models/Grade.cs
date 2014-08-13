using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class Grade
    {
        [Key]
        public int GradeId { get; set; }

        public string GradeLetter { get; set; }

        public virtual List<Enrollment> EnrollmentList { get; set; }
    }
}