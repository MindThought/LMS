using LMS.Models;
using LMS.SpecialBehaviour;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
	[CustomAuthorize(Roles = "Teacher")]
    public class CoursesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();

        public CoursesController()
        {
        }

        public CoursesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

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
            ViewBag.CourseId = course.Id;
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

            if (course == null)
            {
                return HttpNotFound();
            }
            ViewBag.CourseId = id;
            return PartialView(course.Modules);
        }
        [Authorize]
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

            List<Activity> activities = new List<Activity>();
            foreach (var module in modules)
            {
                activities.AddRange(module.Activities.Where(a => a.EndTime > start && a.StartTime < end).ToList());
            }
            if (activities.Count == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NoContent);
            }

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

        public ActionResult Upload(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = db.Courses.Where(c => c.Id == id).First();
            return View(course);
        }

        [HttpPost]
        public ActionResult SaveDocument(List<HttpPostedFileBase> fileUpload, string name, string desc, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Course course = db.Courses.Where(c => c.Id == id).First();

			if (course == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}

            if (fileUpload.Count >= 1)
            {
                foreach (var file in fileUpload)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 10; //10 MB
                        string[] AllowedFileExtensions = new string[] { ".docx", ".pdf", ".pptx", ".xlsx", ".txt", ".zip", ".7z" };
						string extension = Path.GetExtension(file.FileName);
						if (!AllowedFileExtensions.Contains(extension))
                        {
                            ViewBag.Message = "Filen ska vara av en av följande typer: " + string.Join(", ", AllowedFileExtensions) + ".";
                        }
                        else if (file.ContentLength > MaxContentLength)
                        {
							ViewBag.Message = "Filen får inte vara större än 10 MB";
						}
                        else
                        {
                            DateTime time = DateTime.Now;
                            string fileName = name + " - " + time.ToString("yyyyMMddHHmmss") + extension;

                            var thePath = Path.Combine(Server.MapPath("~/Attach/Document"), fileName);
                            if (System.IO.File.Exists(thePath))
                            {
                                ViewBag.Message = "Kollision inträffade, testa igen och köp en lotteribiljett.";
                                return View("Details", course);
                            }

                            file.SaveAs(Path.Combine(Server.MapPath("~/Attach/Document"), fileName));
                            //It has to be this way!
                            var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
                            var second = db.Users.Find(user.Id);
                            course.Documents.Add(new Document { Description = desc, Name = name, FilePath = name + extension, Uploader = second, Uploaded = time });
                            ModelState.Clear();
                            db.SaveChanges();
                            ViewBag.Message = "Filuppladdningen lyckades";
                        }
                    }
                }
            }
            return View("Details", course);
        }
        [Authorize]
        public ActionResult ShowDocuments(int id)
        {
            var documents = db.Courses.Where(c => c.Id == id).First().Documents.ToList();
            ViewBag.Id = id;
            return PartialView(documents);
        }
        [Authorize]
        public ActionResult Download(int id)
        {
            Document document = db.Documents.Where(d => d.Id == id).First();
            string fileName = document.Name + " - " + document.Uploaded.ToString("yyyyMMddHHmmss") + document.FilePath.Substring(document.FilePath.LastIndexOf('.'));
            var FileVirtualPath = "~/Attach/Document/" + fileName;
            return File(FileVirtualPath, "application/octet-stream", document.Name + document.FilePath.Substring(document.FilePath.LastIndexOf('.')));
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
            ViewBag.DelStr = course.Name;
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
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


        public ActionResult DeleteVerify(int id)
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
