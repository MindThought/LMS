using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
	public class Course
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime StartDate { get; set; }

		public virtual List<ApplicationUser> Students { get; set; }
		public virtual List<Module> Modules { get; set; }
		public virtual List<Document> Documents { get; set; }
	}
}