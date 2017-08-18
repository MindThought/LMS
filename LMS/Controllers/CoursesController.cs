using LMS.Models;
using LMS.SpecialBehaviour;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace LMS.Controllers
{
	[CustomAuthorize(Roles = "Teacher" )]
	public class CoursesController : Controller
	{
		private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index(string SearchText)
		{
			if (String.IsNullOrWhiteSpace(SearchText))
			{
				return View(db.Courses.ToList());
			}
			var result = db.Courses.Where(c => c.Name.Contains(SearchText));
			return View(result.ToList());
		}
		// GET: Courses/Details/5
		[Authorize]
		public ActionResult Details(int? id)
		{
			Course course = null;
			if (id == null)
			{
				if (User.IsInRole("Student"))
				{
					id = (int)db.Users.Where(s => s.UserName == User.Identity.Name).FirstOrDefault().CourseId;
				}
				else
				{
					return RedirectToAction("Index");
				}
			}
			course = db.Courses.Find(id);
			if (course == null)
			{
				return HttpNotFound();
			}
			return View(course);
		}
		[Authorize]
		public ActionResult CourseStudents(int? id)
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
			var output = course.Students.OrderBy(s => s.UserName);
			return PartialView(output);
		}
		[HttpGet]
		[Authorize]
		public ActionResult CourseModules(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Course course = db.Courses.Find(id);
			ViewBag.CourseId = id;
			if (course == null)
			{
				return HttpNotFound();
			}

			return PartialView(course.Modules);
		}

        public ActionResult Schedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Where(c => c.Id == id).FirstOrDefault();
            if (course == null)
            {
                return HttpNotFound();
            }

            DateTime today = DateTime.Now;
            today = new DateTime(today.Year, today.Month, today.Day);
            DateTime start = today;
            switch (today.DayOfWeek)
            {

                case DayOfWeek.Monday:

                    break;
                case DayOfWeek.Tuesday:
                    start = new DateTime(start.Year, start.Month, start.Day - 1);
                    break;
                case DayOfWeek.Wednesday:
                    start = new DateTime(start.Year, start.Month, start.Day - 2);
                    break;
                case DayOfWeek.Thursday:
                    start = new DateTime(start.Year, start.Month, start.Day - 3);
                    break;
                case DayOfWeek.Friday:
                    start = new DateTime(start.Year, start.Month, start.Day - 4);
                    break;
                case DayOfWeek.Saturday:
                    start.AddDays(2);
                    break;
                case DayOfWeek.Sunday:
                    start.AddDays(1);
                    break;
                default:
                    break;
            }
            DateTime end = start.AddDays(5);


            List<Module> modules = course.Modules.Where(m => m.EndDate > start && m.StartDate < end).ToList();
            if (modules.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }
            return PartialView(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Course course = db.Courses.Find(id);
            
            foreach (var item in db.Users.Where(u => u.CourseId == course.Id))
            {
                db.Users.Remove(item);
            }

            foreach (var module in db.Modules.Where(m => m.CourseId == course.Id).ToList())
            {
                foreach (var activity in db.Activities.Where(a => a.ModuleId == module.Id).ToList())
                {
                    db.Activities.Remove(activity);
                }
                db.Modules.Remove(module);
            }

            db.Courses.Remove(course);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

		// GET: Courses/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: Courses/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "Id,Name,Description,StartDate")] Course course)
		{
			if (ModelState.IsValid)
			{
				db.Courses.Add(course);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(course);
		}

		// GET: Courses/Edit/5
		public ActionResult Edit(int? id)
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
			return View(course);
		}

		// POST: Courses/Edit/5
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "Id,Name,Description,StartDate")] Course course)
		{
			if (ModelState.IsValid)
			{
				db.Entry(course).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(course);
		}

		// GET: Courses/Delete/5
		public ActionResult Delete(int? id)
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
			return View(course);
		}

		// POST: Courses/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]

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
