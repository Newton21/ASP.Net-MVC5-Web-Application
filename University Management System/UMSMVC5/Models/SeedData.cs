using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace UMSMVC5.Models
{
    public class SeedData : DropCreateDatabaseIfModelChanges<UMSMVC5DbContext>
    {
        protected override void Seed(UMSMVC5DbContext context)
        {
            context.Departments.Add(new Department { DepartmentCode = "CSE", DepartmentName = "Computer Science and Engineering" });
            context.Departments.Add(new Department { DepartmentCode = "EEE", DepartmentName = "Electrical & Electronics Engineering" });
            context.SaveChanges();

            context.Semesters.Add(new Semester { SemesterName = "First" });
            context.Semesters.Add(new Semester { SemesterName = "Second" });
            context.Semesters.Add(new Semester { SemesterName = "Third" });
            context.Semesters.Add(new Semester { SemesterName = "Forth" });
            context.Semesters.Add(new Semester { SemesterName = "Fifth" });
            context.Semesters.Add(new Semester { SemesterName = "Sixth" });
            context.Semesters.Add(new Semester { SemesterName = "Seventh" });
            context.Semesters.Add(new Semester { SemesterName = "Eighth" });

            context.SaveChanges();

            context.Designations.Add(new Designation { DesignationName = "Professor" });
            context.Designations.Add(new Designation { DesignationName = "Lecturer" });
            context.Designations.Add(new Designation { DesignationName = "Assistant Profrssor" });
            context.Designations.Add(new Designation { DesignationName = "Senior Lecturer" });

            context.SaveChanges();

            context.Grades.Add(new Grade { GradeLetter = "Result not published yet" });
            context.Grades.Add(new Grade { GradeLetter = "A+" });
            context.Grades.Add(new Grade { GradeLetter = "A" });
            context.Grades.Add(new Grade { GradeLetter = "A-" });
            context.Grades.Add(new Grade { GradeLetter = "B+" });
            context.Grades.Add(new Grade { GradeLetter = "B" });
            context.Grades.Add(new Grade { GradeLetter = "B-" });
            context.Grades.Add(new Grade { GradeLetter = "C+" });
            context.Grades.Add(new Grade { GradeLetter = "C" });
            context.Grades.Add(new Grade { GradeLetter = "D" });
            context.Grades.Add(new Grade { GradeLetter = "F" });
            context.SaveChanges();
        }
    }
}