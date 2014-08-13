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
    public class EnrollmentController : Controller
    {
        private UMSMVC5DbContext db = new UMSMVC5DbContext();

        // GET: /Enrollment/
        public ActionResult Index()
        {
            var enrollments = db.Enrollments.Include(e => e.Course).Include(e => e.Grade);
            return View(enrollments.ToList());
        }

        public ActionResult StudentInfo(string RegistrationId)
        {

            var count = db.Students.Count(s => s.RegistrationId == RegistrationId);

            if (count > 0)
            {
                var student = db.Students.Where(s => s.RegistrationId == RegistrationId).FirstOrDefault();
                return PartialView("_StudentInfo", student);
            }

            else
            {

                return PartialView("_NoStudentFound");
            }


        }

        // GET: /Enrollment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: /Enrollment/Create
        public ActionResult Create()
        {
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode");
            //ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter");
            return View();
        }

        // POST: /Enrollment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="EnrollmentId,RegistrationId,CourseId,EnrollmentDate,GradeId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
               
               
                    Enrollment aEnrollment= db.Enrollments.FirstOrDefault(e => (e.RegistrationId == enrollment.RegistrationId) && (e.CourseId == enrollment.CourseId));
                    Student student = db.Students.Where(s=>s.RegistrationId==enrollment.RegistrationId).FirstOrDefault();
                    Course course = db.Courses.Find(enrollment.CourseId);
                    if (aEnrollment == null)
                    {
                        enrollment.GradeId = 1;
                        db.Enrollments.Add(enrollment);
                        int save = db.SaveChanges();
                        if (save > 0)
                        {
                            this.ShowMessage(MessageType.Success, "This Course : " + course.CourseCode + " has been successfully Enrolled to student " +student.RegistrationId, true);
                            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
                            return View();
                        }
                          
                    }
                    else
                    {
                        this.ShowMessage(MessageType.Error, "This Course : " + course.CourseCode + " is already Enrolled to student " + student.RegistrationId, true);
                        ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
                        return View(enrollment);
                    }


                    ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
        
                return View(enrollment);

            }

            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
            //ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
            return View(enrollment);
        }

        // GET: /Enrollment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
            //ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
            return View(enrollment);
        }

        // POST: /Enrollment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="EnrollmentId,RegistrationId,CourseId,EnrollmentDate,GradeId")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CourseId = new SelectList(db.Courses, "CourseId", "CourseCode", enrollment.CourseId);
            //ViewBag.GradeId = new SelectList(db.Grades, "GradeId", "GradeLetter", enrollment.GradeId);
            return View(enrollment);
        }

        // GET: /Enrollment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: /Enrollment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
