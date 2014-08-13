using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC5.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Course Code Can Not Be Empty")]
        [Remote("CheckCourseCode", "Course", ErrorMessage = "Course Code already exists")]
        [Display(Name = "Course Code")]
        public string CourseCode { get; set; }


        [Required(ErrorMessage = "Course Title Can Not Be Empty")]
        [Remote("CheckCourseTitle", "Course", ErrorMessage = "Course Title already exists")]
        [Display(Name = "Course Title")]
        public string CourseTitle { get; set; }

        [Required(ErrorMessage = "Credit")]
        [Display(Name = "Credit")]
        public double CourseCredit { get; set; }
        public string Description { get; set; }

        public string AssignCourse { set; get; }

        [Required(ErrorMessage = "Please Select Department Name")]
        [Display(Name = "Department Name")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Please Select Semester")]
        [Display(Name = "Semester")]
        public int SemesterId { get; set; }



        //[ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("SemesterId")]
        public virtual Semester Semester { get; set; }
      
    }
}