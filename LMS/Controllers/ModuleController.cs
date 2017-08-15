﻿using LMS.Models;
using LMS.SpecialBehaviour;
using System;
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
			var currentTime = DateTime.Now;
			var activities = module.Activities.OrderBy(a => a.StartTime).ToList();

			return PartialView(activities);
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


