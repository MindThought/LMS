using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
	public class Document
	{
		[Key]
		public int Id { get; set; }
		[DisplayName("Namn")]
		public string Name { get; set; }
		[DisplayName("Beskrivning")]
		public string Description { get; set; }
		[DisplayName("FilNamn")]
		public string FilePath { get; set; }
		[DisplayName("Uppladdad")]
		public DateTime Uploaded { get; set; }
		[Required]
		[DisplayName("Uppladdare")]
		public ApplicationUser Uploader { get; set; }
		public DateTime? DeadLine { get; set; }
	}
}