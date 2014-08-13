using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC5.Models
{
    public class Student
    {
        [Key]
        public int StudentId { get; set; }

        public string RegistrationId { get; set; }

        [Required(ErrorMessage = "Name Can Not Be Empty")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Can Not Be Empty")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Enter a Valid Email")]
        [Remote("CheckStudentEmail", "Student", ErrorMessage = "This Email already exists")]
        public string Email { get; set; }

        [Display(Name = "Contact No")]
        public string ContactNo { get; set; }

        public string Address { get; set; }

        [Required(ErrorMessage = "Please Select Department")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public virtual Department Department { get; set; }

        [Display(Name = "Registration Date")]
        public DateTime RegistrationDate { get; set; }
    }
}