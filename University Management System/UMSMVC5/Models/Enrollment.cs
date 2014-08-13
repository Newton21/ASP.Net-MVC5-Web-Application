using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class Enrollment
    {
        [Key]
        public int EnrollmentId { get; set; }

        [Required]
        [Display(Name = "Student Registration Id")]
        public string RegistrationId { get; set; }

           [Display(Name = "Course")]
           [Required]
        public int CourseId { get; set; }

        [Display(Name = "Date")]
        public DateTime EnrollmentDate { get; set; }

         [Display(Name = "Grade")]
        public int GradeId { get; set; }


        //[ForeignKey("StudentId")]
        //public virtual Student Student { get; set; }

        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }

        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }

    }
}