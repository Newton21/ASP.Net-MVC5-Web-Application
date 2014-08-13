using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UMSMVC5.Models;
using UMSMVC5.Notifications;

namespace UMSMVC5.Controllers
{
    public class CourseAssignController : Controller
    {
        private UMSMVC5DbContext db = new UMSMVC5DbContext();

        // GET: /CourseAssign/
        public ActionResult Index()
        {
            var courseassigns = db.CourseAssigns.Include(c => c.Course).Include(c => c.Teacher);
            return View(courseassigns.ToList());
        }


        public ViewResult ViewCourseStatus(int? departmentID)
        {
            ViewBag.DepartmentID = new SelectList(db.Departments, "DepartmentId", "DepartmentCode");

            var courselist = new List<Course>();
            if (departmentID != null)
            {
                courselist = db.Courses.Include(c => c.Semester).Where(c => c.DepartmentId == departmentID).ToList();
            }
            else
            {
                courselist = db.Courses.Include(c => c.Semester).ToList();
            }

            //var coursedbset = db.Courses.Include(c => c.Department).Include(c => c.Semester);
            return View(courselist.ToList());
        }

        public PartialViewResult CourseFilter(int? departmentID)
        {
            var courselist=new List<Course>();
            if (departmentID != null)
            {
                courselist = db.Courses.Include(c => c.Semester).Where(c => c.DepartmentId == departmentID).ToList();
            }
            else
            {
                courselist = db.Courses.Include(c => c.Semester).ToList();
            }
            return PartialView("~/Views/CourseAssign/_CourseFilter.cshtml", courselist);
        }


        public JsonResult GetDepartmentList()
        {

            db.Configuration.ProxyCreationEnabled = false;

            var departmentlist = db.Departments.ToList();

            return Json(departmentlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTeacherList(int? DepartmentId)
        {

            db.Configuration.ProxyCreationEnabled = false;

            var teacherlist = db.Teachers.Where(t=>t.DepartmentId==DepartmentId);

            return Json(teacherlist.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseList(int? DepartmentId)
        {

            db.Configuration.ProxyCreationEnabled = false;

            var courseList = db.Courses.Where(t => t.DepartmentId == DepartmentId);

            return Json(courseList.ToList(), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetTeacherCreditInfo(int? teacherId)
        {
            var aTeacher = new Teacher();
            if (teacherId != null)
            {
                aTeacher = db.Teachers.Where(t => t.TeacherId == teacherId).FirstOrDefault();
            }
            return PartialView("~/Views/CourseAssign/_TeacherCreditInfo.cshtml", aTeacher);
        }

        public PartialViewResult GetCourseInfo(int? courseId)
        {
            var aCourse = new Course();
            if (courseId != null)
            {
                aCourse = db.Courses.Where(c => c.CourseId == courseId).FirstOrDefault();
            }
            return PartialView("~/Views/CourseAssign/_CourseInfor.cshtml", aCourse);
        }


        // GET: /CourseAssign/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssign courseassign = db.CourseAssigns.Find(id);
            if (courseassign == null)
            {
                return HttpNotFound();
            }
            return View(courseassign);
        }

        // GET: /CourseAssign/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName");
            return View();
        }

        // POST: /CourseAssign/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CourseAssignId,CourseId,TeacherId")] CourseAssign courseassign)
        {
            if (ModelState.IsValid)
            {
                CourseAssign acCourseAssign = db.CourseAssigns.FirstOrDefault(ca => ca.CourseId == courseassign.CourseId);
                Course aCourse = db.Courses.FirstOrDefault(c => c.CourseId == courseassign.CourseId);
                Teacher aTeacher = db.Teachers.FirstOrDefault(t => t.TeacherId == courseassign.TeacherId);

                if (acCourseAssign != null)
                {

                    Teacher assignTeacher = db.Teachers.FirstOrDefault(ast => ast.TeacherId == acCourseAssign.TeacherId);
                    if (assignTeacher != null)
                    {
                        if (aCourse != null)
                        {

                            this.ShowMessage(MessageType.Error, "This Course : " + aCourse.CourseCode + " has already been assaigned to " + assignTeacher.TeacherName, true);
                          
                            ViewBag.CourseId = new SelectList(db.Courses.Where(c => c.CourseId == aCourse.CourseId), "CourseId", "CourseCode");
                            ViewBag.TeacherId = new SelectList(db.Teachers.Where(t => t.TeacherId == aTeacher.TeacherId), "TeacherId", "TeacherName");

                            return View();
                        }
                    }
                }

                aTeacher.CreditsHaveTaken += aCourse.CourseCredit;
                aTeacher.CreditsRemaining -= aCourse.CourseCredit;
                db.CourseAssigns.Add(courseassign);
                aCourse.AssignCourse = aTeacher.TeacherName;
              
                int save =db.SaveChanges();
                if (save > 0)
                {
                    this.ShowMessage(MessageType.Success, "This Course" +aCourse.CourseCode
                        + " has been assigned to Teacher :- " + aTeacher.TeacherName
                        + " successfully .", true);

                    return RedirectToAction("Index");
                }
                return RedirectToAction("Index");
            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseassign.CourseId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseassign.TeacherId);
            return View(courseassign);
        }

        // GET: /CourseAssign/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssign courseassign = db.CourseAssigns.Find(id);
            if (courseassign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseassign.CourseId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseassign.TeacherId);
            return View(courseassign);
        }

        // POST: /CourseAssign/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CourseAssignId,CourseId,TeacherId")] CourseAssign courseassign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(courseassign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", courseassign.CourseId);
            ViewBag.TeacherId = new SelectList(db.Teachers, "TeacherId", "TeacherName", courseassign.TeacherId);
            return View(courseassign);
        }

        // GET: /CourseAssign/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CourseAssign courseassign = db.CourseAssigns.Find(id);
            if (courseassign == null)
            {
                return HttpNotFound();
            }
            return View(courseassign);
        }

        // POST: /CourseAssign/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CourseAssign courseassign = db.CourseAssigns.Find(id);
            db.CourseAssigns.Remove(courseassign);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
