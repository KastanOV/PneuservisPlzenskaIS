using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PneuservisISMVC;


namespace PneuservisISMVC.Controllers
{
    public class BazarTiresController : Controller
    {
        private DefaultDataModel db = new DefaultDataModel();

        // GET: BazarTires
        public ActionResult Index()
        {
            
            var bazarTires = db.BazarTires.Include(b => b.Customer);
            return View(bazarTires.ToList());
        }

        // GET: BazarTires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            BazarTire bazarTires = db.BazarTires.Find(id);
            if (bazarTires == null)
            {
                return HttpNotFound();
            }
            return View(bazarTires);
        }

        // GET: BazarTires/Create
        public ActionResult Create()
        {
            ViewBag.Customers_id = new SelectList(db.Customers, "id", "email");
            return View();
        }

        // POST: BazarTires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Manufacturer,Size,Type,Description,Customers_id")] BazarTire bazarTires)
        {
            if (ModelState.IsValid)
            {
                db.BazarTires.Add(bazarTires);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Customers_id = new SelectList(db.Customers, "id", "email", bazarTires.Customers_id);
            return View(bazarTires);
        }

        // GET: BazarTires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BazarTire bazarTires = db.BazarTires.Find(id);
            if (bazarTires == null)
            {
                return HttpNotFound();
            }
            ViewBag.Customers_id = new SelectList(db.Customers, "id", "email", bazarTires.Customers_id);
            return View(bazarTires);
        }

        // POST: BazarTires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Manufacturer,Size,Type,Description,Customers_id")] BazarTire bazarTires)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bazarTires).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Customers_id = new SelectList(db.Customers, "id", "email", bazarTires.Customers_id);
            return View(bazarTires);
        }

        // GET: BazarTires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BazarTire bazarTires = db.BazarTires.Find(id);
            if (bazarTires == null)
            {
                return HttpNotFound();
            }
            return View(bazarTires);
        }

        // POST: BazarTires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BazarTire bazarTires = db.BazarTires.Find(id);
            db.BazarTires.Remove(bazarTires);
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
