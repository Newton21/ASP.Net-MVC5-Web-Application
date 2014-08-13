using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UMSMVC5.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "Department Code Can Not Be Empty")]
        [Remote("CheckDepatmentCode", "Department", ErrorMessage = "Department Code already exists", AdditionalFields = "InitialCode")]
        [Display(Name="Department Code")]
        public string DepartmentCode { get; set; }

        [Required(ErrorMessage = "Department Name Can Not Be Empty")]
        [Remote("CheckDepatmentName", "Department", ErrorMessage = "Department Name already exists")]
        [Display(Name="Department Name")]
        public string DepartmentName { get; set; }


        public virtual List<Course> CourseList { get; set; }
        public virtual List<Student> StudentList { get; set; }
        public virtual List<Teacher> TeacherList { get; set; }
    }
}