using LMS.Models;
using LMS.SpecialBehaviour;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

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

		public ActionResult Schedule(int? id)
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
			var activities = module.Activities.Where(a => a.StartTime < end && a.EndTime > start).ToList();
			List<Period> periodes = new List<Period>(); 
			foreach (var item in activities)
			{
				int startHour = 0;
				int startMinute = 0;
				int endHour = 0;
				int endMinute = 0;
				int days = item.EndTime.Day - item.StartTime.Day;

				if (days == 0)//If it is only during one day
				{
					if (item.StartTime.Hour <= 8)
					{
						startHour = 8;
						startMinute = 30;
					}
					else
					{
						startHour = item.StartTime.Hour;
						startMinute = item.StartTime.Minute;
					}
					if (item.EndTime.Hour > 17)
					{
						endHour = 17;
						endMinute = 0;
					}
					else
					{
						endHour = item.EndTime.Hour;
						endMinute = item.EndTime.Minute;
					}
					Period period0 = new Period
					{
						ModuleId = item.Id,
						Day = item.StartTime.Day - start.Day,
						Name = item.Name,
						StartHour = startHour,
						StartMinute = startMinute,
						EndHour = endHour,
						EndMinute = endMinute
					};
					if (period0.Day >= 0)
					{
						periodes.Add(period0);
					}
				}
				if (days > 0)
				{
					if (item.StartTime.Hour <= 8)
					{
						startHour = 8;
						startMinute = 30;
					}
					else
					{
						startHour = item.StartTime.Hour;
						startMinute = item.StartTime.Minute;
					}
					endHour = 17;
					endMinute = 0;
					Period period0 = new Period
					{
						ModuleId = item.Id,
						Day = item.StartTime.Day - start.Day,
						Name = item.Name,
						StartHour = startHour,
						StartMinute = startMinute,
						EndHour = endHour,
						EndMinute = endMinute
					};
					if (period0.Day < 0)
					{
						period0.Day = 0;
						period0.StartHour = 8;
						period0.StartMinute = 30;
					}
					periodes.Add(period0);
				}
				if (days > 1)
				{
					for (int i = 1; i <= days; i++)
					{
						startHour = 8;
						startMinute = 30;
						endHour = 17;
						endMinute = 0;
						Period period = new Period
						{
							ModuleId = item.Id,
							Day = item.StartTime.Day - start.Day + i,
							Name = item.Name,
							StartHour = startHour,
							StartMinute = startMinute,
							EndHour = endHour,
							EndMinute = endMinute
						};
						if (period.Day < 5 && period.Day >= 0)
						{
							periodes.Add(period);
						}
					}
				}
				if (days > 0)
				{
					startHour = 8;
					startMinute = 30;

					if (item.EndTime.Hour > 17)
					{
						endHour = 17;
						endMinute = 0;
					}
					else
					{
						endHour = item.EndTime.Hour;
						endMinute = item.EndTime.Minute;
					}
					Period periodN = new Period
					{
						ModuleId = item.Id,
						Day = item.EndTime.Day - start.Day,
						Name = item.Name,
						StartHour = startHour,
						StartMinute = startMinute,
						EndHour = endHour,
						EndMinute = endMinute
					};
					if (periodN.Day < 5)
					{
						periodes.Add(periodN);
					}
				}
			}
			return PartialView(periodes);
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


