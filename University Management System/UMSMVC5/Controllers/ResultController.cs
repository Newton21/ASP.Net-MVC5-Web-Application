using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using UMSMVC5.Models;
using UMSMVC5.Notifications;
using UMSMVC5.Reports;

namespace UMSMVC5.Controllers
{
    public class ResultController : Controller
    {
        private UMSMVC5DbContext db = new UMSMVC5DbContext();
        //
        // GET: /Result/
        //public ActionResult Index()
        //{
        //    return View();
        //}


        public ActionResult ResultEntry()
        {
        
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");
            var gradelist = db.Grades.Where(g => g.GradeId != 1);
            ViewBag.GradeId = new SelectList(gradelist, "GradeId", "GradeLetter");
            return View();
        }

        public JsonResult GetCourseList(string RegistrationId)
            {

            db.Configuration.ProxyCreationEnabled = false;

            var courseList = db.Enrollments.Where(e => e.RegistrationId == RegistrationId).Include(c => c.Course);

            return Json(courseList.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResultEntry([Bind(Include = "EnrollmentId,RegistrationId,CourseId,EnrollmentDate,GradeId")] Enrollment enrollment)
        {

            if (ModelState.IsValid)
            {
                Enrollment anEnrollment = db.Enrollments.FirstOrDefault(e => (e.CourseId == enrollment.CourseId) && (e.RegistrationId == enrollment.RegistrationId));
                Student student = db.Students.Where(s => s.RegistrationId == enrollment.RegistrationId).FirstOrDefault();
                Course course = db.Courses.Find(enrollment.CourseId);

                if (anEnrollment.GradeId == 1)
                {
                    anEnrollment.GradeId = enrollment.GradeId;
                    int save = db.SaveChanges();
                    if (save > 0)
                    {
                        this.ShowMessage(MessageType.Success, "Result For This Course " + course.CourseCode + " has been saved successfully ", true);
                        ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
                        ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
                        return View();

                    }
                    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
                    ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);

                    return View();
                }
                this.ShowMessage(MessageType.Error, "Result For This Course " + course.CourseCode + " has already been published ", true);
                ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
                ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
                return View();

            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
            ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
            return View();
        }


        public ActionResult StudentInfo(string RegistrationId)
        {

            var count = db.Students.Count(s => s.RegistrationId == RegistrationId);

            if (count > 0)
            {
                var student = db.Students.Where(s => s.RegistrationId == RegistrationId).FirstOrDefault();
                return PartialView("~/Views/Enrollment/_StudentInfo.cshtml", student);
            }

            else
            {

                return PartialView("~/Views/Enrollment/_NoStudentFound.cshtml");
            }


        }


        public ActionResult ViewResult()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewResult(Enrollment enrollment)
        {


            List<ResultViewModel> resultviewlist = new List<ResultViewModel>();

            var enrollmentlist = db.Enrollments.Where(e => e.RegistrationId == enrollment.RegistrationId).ToList();
           
                foreach (var getenrollment in enrollmentlist)
                {
                    ResultViewModel aResultViewModel = new ResultViewModel();
                    aResultViewModel.CourseCode = getenrollment.Course.CourseCode;
                    aResultViewModel.CourseName = getenrollment.Course.CourseTitle;
                    aResultViewModel.Grade = getenrollment.Grade.GradeLetter;

                    var getstudent = db.Students.Where(e => e.RegistrationId == getenrollment.RegistrationId).ToList();
                    foreach (var viewResult in getstudent)
                    {
                        aResultViewModel.StudentRegNo = viewResult.RegistrationId;
                        aResultViewModel.StudentName = viewResult.Name;
                        aResultViewModel.Email = viewResult.Email;
                        aResultViewModel.DepartmentName = viewResult.Department.DepartmentName;

                      
                }

                    resultviewlist.Add(aResultViewModel);
            }
           
            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "ViewResultReport.rpt"));
            rd.SetDataSource(resultviewlist.ToList());
            TableLogOnInfo aInfo = new TableLogOnInfo();

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();

            try
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "application/pdf", "Result.pdf");
            }
            catch (Exception ex)
            {
                throw;
            }



        }
	}
}