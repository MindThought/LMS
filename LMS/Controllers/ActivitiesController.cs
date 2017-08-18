using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMS.Models;
using LMS.SpecialBehaviour;

namespace LMS.Controllers
{
    [CustomAuthorize(Roles = "Teacher")]
    public class ActivitiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Activities
        [Authorize]
        public async Task<ActionResult> Index()
        {
            return View(await db.Activities.ToListAsync());
        }

        // GET: Activities/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = await db.Activities.FindAsync(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // GET: Activities/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Activities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Create([Bind(Include = "Id,Type,Name,Description,StartTime,EndTime")] Activity activity, int ModuleId)
        {
            var module = db.Modules.Find(ModuleId);
            var course = module.Course;
            if (activity.StartTime >= activity.EndTime)
            {
                ModelState.AddModelError("", "Fel, starttiden kan inte vara senare än sluttiden");
                return View(activity);
            }

            var moduleOverlapST = course.Modules.Any(m => m.StartDate < activity.StartTime && m.EndDate > activity.StartTime && m != module);
            var moduleOverlapET = course.Modules.Any(m => m.StartDate < activity.EndTime && m.EndDate > activity.EndTime && m != module);
            var activityOverlapST = module.Activities.Any(a => a.StartTime < activity.StartTime && a.EndTime > activity.StartTime && a != activity);
            var activityOverlapET = module.Activities.Any(a => a.StartTime < activity.EndTime && a.EndTime > activity.EndTime && a != activity);

            if (moduleOverlapST || moduleOverlapET)
            {
                ModelState.AddModelError("", "Fel, Tiden på aktiviteten överlappar en annan modul");
                return View(activity);
            }

            if (activityOverlapST || activityOverlapET)
            {
                ModelState.AddModelError("", "Fel, Tiden på aktiviteten överlappar en annan aktivitet i denna modul");
                return View(activity);
            }

            if (ModelState.IsValid)
            {
                db.Activities.Add(activity);
                activity.Module = module;
                activity.ModuleId = module.Id;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", "Module", new { id = ModuleId });
            }
            return View(activity);
        }

        // GET: Activities/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = await db.Activities.FindAsync(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Type,Name,Description,StartTime,EndTime")] Activity activity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(activity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(activity);
        }

        // GET: Activities/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Activity activity = await db.Activities.FindAsync(id);
            if (activity == null)
            {
                return HttpNotFound();
            }
            return View(activity);
        }

        // POST: Activities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Activity activity = await db.Activities.FindAsync(id);
            var module = await db.Modules.FindAsync(activity.Module.Id);
            module.Activities.Remove(activity);
            db.Activities.Remove(activity);
            await db.SaveChangesAsync();
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
