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
    public class StudentController : Controller
    {
        private UMSMVC5DbContext db = new UMSMVC5DbContext();

        // GET: /Student/
        public ActionResult Index()
        {
            var students = db.Students.Include(s => s.Department);
            return View(students.ToList());
        }

        // GET: /Student/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: /Student/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode");
            return View();
        }

        // POST: /Student/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="StudentId,RegistrationId,Name,Email,ContactNo,Address,DepartmentId,RegistrationDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                int id = db.Students.Count(s => (s.DepartmentId == student.DepartmentId)
                    && (s.RegistrationDate.Year == student.RegistrationDate.Year)) + 1;
                Department aDepartment= db.Departments.Where(d => d.DepartmentId == student.DepartmentId).FirstOrDefault();
                
                student.RegistrationId = aDepartment.DepartmentCode +"-" + student.RegistrationDate.Year.ToString() + "-";

                if (id < 10)
                {
                    student.RegistrationId += "00" + id.ToString();
                }
                   
                else if (id < 100)
                {
                    student.RegistrationId += "0" + id.ToString();
                }

                else
                {
                    student.RegistrationId += id.ToString();
                }
                   
                db.Students.Add(student);
                int save = db.SaveChanges();

                if (save > 0)
                {
                    this.ShowMessage(MessageType.Success,"Student has been saved successfully with Registration No :  "+ student.RegistrationId, true);
                }

                return RedirectToAction("Index");
            }

            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", student.DepartmentId);
            return View(student);
        }

        // GET: /Student/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", student.DepartmentId);
            return View(student);
        }

        // POST: /Student/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="StudentId,RegistrationId,Name,Email,ContactNo,Address,DepartmentId,RegistrationDate")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "DepartmentId", "DepartmentCode", student.DepartmentId);
            return View(student);
        }

        // GET: /Student/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: /Student/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        public JsonResult CheckStudentEmail(string email)
        {
            var result = db.Students.Count(s => s.Email == email) == 0;
            return Json(result, JsonRequestBehavior.AllowGet);
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
