using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
	public class Document
	{
		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string Path { get; set; }
		public DateTime Uploaded { get; set; }
		public virtual ApplicationUser Uploader { get; set; }
		[Required]
		public string Uploader_Id { get; set; }
		public DateTime? DeadLine { get; set; }
	}
}