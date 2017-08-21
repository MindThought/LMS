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
		public string FilePath { get; set; }
		public DateTime Uploaded { get; set; }
		[Required]
		public ApplicationUser Uploader { get; set; }
		public DateTime? DeadLine { get; set; }
	}
}