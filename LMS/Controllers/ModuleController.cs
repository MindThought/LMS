using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.SpecialBehaviour;

namespace LMS.Controllers
{
    [CustomAuthorize(Roles = "Teacher")]
    public class ModuleController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Module
        public ActionResult Index()
        {
            return View(db.Modules.ToList());
        }

        public ActionResult CourseModules(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = id;
            return View(course.Modules);
        }

        // GET: Module/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }

            var monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 1);

            var aktivitetFM = new List<string>();
            var aktivitetEM = new List<string>();
            var weekDays = new List<DayOfWeek>();
            var dates = new List<string>();
 

            for (int i = 0; i < module.Activities.Count; i++)
            {

                if (module.Activities[i].StartTime.Hour < 12)
                {
                    aktivitetFM.Add(module.Activities[i].Name);
                }
                else
                {
                    aktivitetEM.Add(module.Activities[i].Name);
                }

                if (module.Activities[i].EndTime.Hour < 12)
                {
                    aktivitetFM.Add(module.Activities[i].Name);
                }
                else
                {
                    aktivitetEM.Add(module.Activities[i].Name);
                }

                if (!dates.Contains(module.Activities[i].StartTime.ToString("yyyy-MM-dd")))
                {
                    dates.Add(module.Activities[i].StartTime.ToString("yyyy-MM-dd"));
                    weekDays.Add(module.Activities[i].StartTime.DayOfWeek);
                }
                //if (module.Activities.Where(a => a.StartTime.ToString("yyyy-MM-dd") == module.Activities[i].StartTime.ToString("yyyy-MM-dd")).ToList().Count > 2)
                //{
                //    weekDays.Add(module.Activities[i].StartTime.DayOfWeek);
                //    IsSameDay = true;
                //}
                //for (int v = 0; v < module.Activities.Count; v++)
                //{
                //    if (startTimeFM[v].ToString("yyyy-MM-dd") == startTimeEM[v].ToString("yyyy-MM-dd"))
                //    {
                //        weekDays.Add(module.Activities[i].StartTime.DayOfWeek);
                //        IsSameDay = true;
                //        break;
                //    }
                //}
            }

            ViewBag.Activities = dates;
            ViewBag.aktivitetFM = aktivitetFM;
            ViewBag.aktivitetEM = aktivitetEM;
            ViewBag.WeekDays = weekDays;

            return View(module);
        }

        // GET: Module/Create
        public ActionResult Create(int? CourseId)
        {
            ViewBag.CourseId = CourseId;
            return View();
        }

        // POST: Module/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                module.Course = db.Courses.Find(module.CourseId);
                db.Modules.Add(module);
                db.SaveChanges();
                return RedirectToAction("Details","Courses",new {id = module.CourseId });
            }

            return View(module);
        }

        // GET: Module/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Module/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate,EndDate,CourseId")] Module module)
        {
            if (ModelState.IsValid)
            {
                db.Entry(module).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Courses", new { id = module.CourseId });
            }
            return View(module);
        }

        // GET: Module/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Module module = db.Modules.Find(id);
            if (module == null)
            {
                return HttpNotFound();
            }
            return View(module);
        }

        // POST: Module/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Module module = db.Modules.Find(id);
            db.Modules.Remove(module);
            db.SaveChanges();
            return RedirectToAction("Details","Courses",new { id = module.CourseId });
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


