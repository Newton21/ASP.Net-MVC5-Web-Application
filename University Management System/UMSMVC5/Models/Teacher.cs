using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC5.Models
{
    public class Teacher
    {
        [Key]
        public int TeacherId { get; set; }

        [Required(ErrorMessage = "Teacher Name Can Not Be Empty")]
        [Display(Name = "Teacher Name")]
        public string TeacherName { get; set; }

        [Required(ErrorMessage = "Email Can Not Be Empty")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",ErrorMessage = "Enter a Valid Email")]
        [Remote("CheckTeacherEmail", "Teacher", ErrorMessage = "This Email already exists")]
        [Display(Name = "Email")]
        public string TeacherEmail { get; set; }

        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage = "Please Select Department")]
        [Display(Name = " Department")]
        public int DepartmentId { get; set; }

         [Required(ErrorMessage = "Please Select Designation")]
         [Display(Name = "Designation")]
        public int DesignationId { get; set; }


        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [ForeignKey("DesignationId")]
        public virtual Designation Designation { get; set; }

        [Required(ErrorMessage = "Credits Can Not Be Empty")]
        [Display(Name = "Credits To Be Taken")]
        public double CreditsToBeTaken { set; get; }
        public double CreditsHaveTaken { set; get; }
        public double CreditsRemaining { set; get; }
       
    }
}