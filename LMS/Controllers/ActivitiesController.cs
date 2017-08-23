using LMS.Models;
using LMS.SpecialBehaviour;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
	[CustomAuthorize(Roles = "Teacher")]
	public class ActivitiesController : Controller
	{
		private ApplicationSignInManager _signInManager;
		private ApplicationUserManager _userManager;
		private ApplicationDbContext db = new ApplicationDbContext();

		public ActivitiesController()
		{
		}

		public ActivitiesController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
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

		public ActionResult Upload(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Activity activity = db.Activities.Where(c => c.Id == id).First();
			return View(activity);
		}

		[HttpPost]
		[Authorize(Roles = "Teacher")]
		public ActionResult SaveDocument(List<HttpPostedFileBase> fileUpload, string name, string desc, int? id)
		{
			List<string> myTempPaths = new List<string>();
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			Activity activity = db.Activities.Where(c => c.Id == id).First();

			if (fileUpload.Count >= 1)
			{
				foreach (var file in fileUpload)
				{
					if (file != null && file.ContentLength > 0)
					{
						int MaxContentLength = 1024 * 1024 * 10; //10 MB
						string[] AllowedFileExtensions = new string[] { ".docx", ".pdf", ".pptx", ".xlsx", ".txt", ".zip", ".7z" };
						if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
						{
							ViewBag.Message = "Filen bör vara av en av följande typer: " + string.Join(", ", AllowedFileExtensions);
						}
						else if (file.ContentLength > MaxContentLength)
						{
							ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " B");
						}
						else
						{
							string extension = Path.GetExtension(file.FileName);

							DateTime time = DateTime.Now;
							string fileName = name + " - " + time.ToString("yyyyMMddHHmmss").ToString() + extension;

							var thePath = Path.Combine(Server.MapPath("~/Attach/Document"), fileName);
							if (System.IO.File.Exists(thePath))
							{
								ViewBag.Message = "Kollision inträffade, testa igen och köp en lotteribiljett.";
								return View("Details", activity);
							}

							file.SaveAs(Path.Combine(Server.MapPath("~/Attach/Document"), fileName));
							//It has to be this way!
							var user = UserManager.FindByNameAsync(User.Identity.Name).Result;
							var second = db.Users.Find(user.Id);
							activity.Documents.Add(new Document { Description = desc, Name = name, FilePath = name + extension, Uploader = second, Uploaded = time });
							ModelState.Clear();
							db.SaveChanges();
							ViewBag.Message = "File uploaded successfully";
						}
					}
				}
			}
			return View("Details", activity);
		}

		public ActionResult ShowDocuments(int id)
		{
			var documents = db.Activities.Where(c => c.Id == id).First().Documents.ToList();
			ViewBag.Id = id;
			return PartialView(documents);
		}

		public ActionResult Download(int id)
		{
			Document document = db.Documents.Where(d => d.Id == id).First();
			string fileName = document.Name + " - " + document.Uploaded.ToString("yyyyMMddHHmmss") + document.FilePath.Substring(document.FilePath.LastIndexOf('.'));
			var FileVirtualPath = "~/Attach/Document/" + fileName;
			return File(FileVirtualPath, "application/octet-stream", document.Name + document.FilePath.Substring(document.FilePath.LastIndexOf('.')));
		}

		// GET: Activities/Create
		[Authorize]
		public ActionResult Create(int ModuleId)
		{
			ViewBag.Module = db.Modules.Find(ModuleId);
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

			var moduleOverlapST = course.Modules.Any(m => m.StartDate <= activity.StartTime && m.EndDate >= activity.StartTime && m != module);
			var moduleOverlapET = course.Modules.Any(m => m.StartDate <= activity.EndTime && m.EndDate >= activity.EndTime && m != module);
			var activityOverlapST = module.Activities.Any(a => a.StartTime <= activity.StartTime && a.EndTime >= activity.StartTime && a != activity);
			var activityOverlapET = module.Activities.Any(a => a.StartTime <= activity.EndTime && a.EndTime >= activity.EndTime && a != activity);

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
		public async Task<ActionResult> Edit([Bind(Include = "Id,Type,Name,Description,StartTime,EndTime")] Activity activity, int? ModuleId)
		{

			if (ModelState.IsValid)
			{
				db.Entry(activity).State = EntityState.Modified;

				var module = db.Modules.Find(ModuleId);
				var course = module.Course;
				if (activity.StartTime >= activity.EndTime)
				{
					ModelState.AddModelError("", "Fel, starttiden kan inte vara senare än sluttiden");
					return View(activity);
				}

				var moduleOverlapST = course.Modules.Any(m => m.StartDate <= activity.StartTime && m.EndDate >= activity.StartTime && m != module);
				var moduleOverlapET = course.Modules.Any(m => m.StartDate <= activity.EndTime && m.EndDate >= activity.EndTime && m != module);
				if (module.Activities != null)
				{
					var activityOverlapST = module.Activities.Any(a => a.StartTime <= activity.StartTime && a.EndTime >= activity.StartTime && a != activity);
					var activityOverlapSTS = module.Activities.Any(a => a.StartTime >= activity.StartTime && a.EndTime <= activity.EndTime && a != activity);
					var activityOverlapET = module.Activities.Any(a => a.StartTime <= activity.EndTime && a.EndTime >= activity.EndTime && a != activity);

					if (activityOverlapST || activityOverlapET || activityOverlapSTS)
					{
						ModelState.AddModelError("", "Fel, Tiden på aktiviteten överlappar en annan aktivitet i denna modul");
						return View(activity);
					}
				}

				if (moduleOverlapST || moduleOverlapET)
				{
					ModelState.AddModelError("", "Fel, Tiden på aktiviteten överlappar en annan modul");
					return View(activity);
				}

				activity.Module = module;
				
				await db.SaveChangesAsync();
				return RedirectToAction("Details",new {id = activity.Id});
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
			return RedirectToAction("Details","Module", new { id = module.Id });
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
