using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HelloWorldMVC.Models;
using Microsoft.Ajax.Utilities;
using Serilog;

namespace HelloWorldMVC.Controllers
{
    public class PeopleController : Controller
    {
        // should this be in a 'using' to close the db conection when out of scope?
        private PeopleDatabaseFirstDBEntities db = new PeopleDatabaseFirstDBEntities();

        // GET: People
        public ActionResult Index()
        {
            // try adding an entry to DB
            /*var context = new PeopleDatabaseFirstDBEntities();
            var person = new Person()
            { 
                FirstName = "Haley",
                TimesMet = 0
            };
            context.People.Add(person);
            context.SaveChanges();*/

            System.Diagnostics.Debug.WriteLine("db = " + db);
            return View(db.People.ToList());
        }

        // GET: People/Details/5
        public ActionResult Details(Person person)
        {
            if (person.FirstName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person p = db.People.Find(person.FirstName);
            if (p == null)
            {
                return HttpNotFound();
            }
            return View(p);
        }

        // GET: People/Create
        public ActionResult Create()
        {

            return View();
        }
        
        // GET: People/Create2
        public ActionResult Create2()
        {

            return View();
        }

        // POST: People/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FirstName,TimesMet")] Person person)
        {
            //check if person already in DB
            var query = db.People.Where(p => p.FirstName == person.FirstName).FirstOrDefault<Person>();



            if (ModelState.IsValid)
            {
                if (query == null) // Person is not in the DB
                {
                    // Add the new person to DB
                    person.TimesMet = 1;
                    db.People.Add(person);
                    Log.Information("Added a new person {FirstName}", person.FirstName);

                    //return RedirectToAction("Index");
                }
                else // Person is already in the DB
                {
                    person = query;
                    person.TimesMet += 1; //Now is an instance of having met 
                    Log.Information("Met {FirstName} {TimesMet} times now", person.FirstName, person.TimesMet);
                }
                db.SaveChanges();
            }

            return View(person);
        }

        // GET: People/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FirstName,TimesMet")] Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: People/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: People/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person person = db.People.Find(id);
            db.People.Remove(person);
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
