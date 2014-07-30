using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PneuservisISMVC;
using PneuservisISMVC.Models;

namespace PneuservisISMVC.Controllers
{
    public class EmployeeAttendancesController : Controller
    {
        private DefaultDataModel db = new DefaultDataModel();

        // GET: EmployeeAttendances
        public ActionResult Wages(int id)
        {
            var employeeAttendance = db.EmployeeAttendances.Where(e => e.Employees_id == id && e.Paid == false);
            ViewBag.SumWages = db.EmployeeAttendances.Where(e => e.Employees_id == id && e.Paid == false).Sum(e => e.Wage);
            var x = db.Employees.Where(e => e.id == id).First();
            ViewBag.Name = x.Lname + " " + x.Fname;
            ViewBag.Id = id;
            return View(employeeAttendance.ToList());
        }

        public ActionResult PayWages([Bind(Include = "id")] int id)
        {
            db.PayWages(id);
            return View();

        }
        /// <summary>
        /// Tak tohle musim zítra komplet predelat abych mohl používat ValidateAntiForgeryToken ViewBag.dreams :)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public ActionResult Attendance(int? id)
        {
            ViewBag.Id = id;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employee emp = db.Employees.Find(id);
            List<EmployeeAttendance> employeeAttendance = db.EmployeeAttendances.Where(Id => Id.Employees_id == id).OrderByDescending(date => date.Arrival).ToList();
            //int xx = db.EmployeeAttendance.SqlQuery("select count(*) from EmployeeAttendance where /'Exit/' is null");
            var ex = db.EmployeeAttendances.Where(exit => exit.Exit == null && exit.Employees_id == id).ToList(); //Kdyz je v ex nejaka hodnota, potom je zamestnanec v praci a bude mu nabidnuto tlacitko Exit
            if (ex.Count() != 0) ViewBag.Arrival = false;
            else ViewBag.Arrival = true;

            emp.EmployeeAttendances = employeeAttendance;

            if (employeeAttendance == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Attendance([Bind(Include = "Employees_id, password")]  )
        //{

        //    return View();
        //}

        public ActionResult Exit(int id)
        {
            db.AttendanceExit(id);
            EmployeeAttendance x = db.EmployeeAttendances.Where(Id => Id.Employees_id == id).OrderByDescending(t => t.Exit).First();
            ViewBag.Time = x.Exit - x.Arrival;
            ViewBag.Paid = x.Wage;
            ViewBag.WageToPaid = db.EmployeeAttendances.Where(Id => Id.Employees_id == id && Id.Paid == false).Sum(s => s.Wage);
            ViewBag.Date = x.Exit;

            return View();
        }
        public ActionResult Arrival(int id)
        {
            db.AttendanceArrival(id);

            return View();
        }

        /// <summary>
        /// Zde vyberu zaměstnance který si dále ve View Details zapíše příchod nebo odchod
        /// </summary>
        /// <param name="Message"></param>
        /// <returns></returns>
        public ActionResult CheckPassword(string Message, int? id)
        {
            ViewBag.Employees_id = new SelectList(db.Employees, "id", "login");
            ViewBag.Message = Message;
            ViewBag.CustomerId = id;
            return View();
        }

        /// <summary>
        /// Tahle metoda porovna zda je heslo platné pokud neni posle hlaseni o chybe do Viewu kdyz je vse OK presmeruje na Controler Details
        /// </summary>
        /// <param name="Employees_id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckPassword([Bind(Include = "Employees_id, password")] int Employees_id, string password)
        {
            //TODO 
            if (ModelState.IsValid)
            {
                var emp = db.Employees.Find(Employees_id);
                if (emp.Password != password)
                {
                    return RedirectToAction("CheckPassword", "EmployeeAttendances", new { Message = "Sorry ale máš tam špatné heslo" });
                }
                //else ViewBag.CustomerId = Employees_id;
                else return RedirectToAction("Attendance", "EmployeeAttendances", new { id = Employees_id });
            }

            return View();
        }

        // GET: EmployeeAttendances/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAttendance employeeAttendance = db.EmployeeAttendances.Find(id);
            if (employeeAttendance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Employees_id = new SelectList(db.Employees, "id", "login", employeeAttendance.Employees_id);
            return View(employeeAttendance);
        }

        // POST: EmployeeAttendances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Arrival,Exit,Wage,Paid,Employees_id")] EmployeeAttendance employeeAttendance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(employeeAttendance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Employees_id = new SelectList(db.Employees, "id", "login", employeeAttendance.Employees_id);
            return View(employeeAttendance);
        }

        // GET: EmployeeAttendances/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EmployeeAttendance employeeAttendance = db.EmployeeAttendances.Find(id);
            if (employeeAttendance == null)
            {
                return HttpNotFound();
            }
            return View(employeeAttendance);
        }

        // POST: EmployeeAttendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EmployeeAttendance employeeAttendance = db.EmployeeAttendances.Find(id);
            db.EmployeeAttendances.Remove(employeeAttendance);
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
