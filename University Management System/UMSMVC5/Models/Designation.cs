using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class Designation
    {
        [Key]
        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public virtual List<Teacher> TeacherList { get; set; }
    }
}