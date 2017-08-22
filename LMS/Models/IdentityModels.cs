using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LMS.Models
{
	// You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
	public class ApplicationUser : IdentityUser
	{
		public virtual Course Course { get; set; }
		public int? CourseId { get; set; }
		[Required]
		public string Name { get; set; }

		public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			userIdentity.AddClaim(new Claim("Name", Name));
			return userIdentity;
		}

	}

	public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
	{
		public ApplicationDbContext()
			: base("DefaultConnection", throwIfV1Schema: false)
		{
			Database.SetInitializer(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
		}

		public DbSet<Course> Courses { get; set; }
		public DbSet<Module> Modules { get; set; }
		public DbSet<Activity> Activities { get; set; }

		public static ApplicationDbContext Create()
		{
			return new ApplicationDbContext();
		}

		public static ApplicationDbContext Delete()
		{
			return new ApplicationDbContext();
		}

		public System.Data.Entity.DbSet<LMS.Models.Document> Documents { get; set; }
	}
}