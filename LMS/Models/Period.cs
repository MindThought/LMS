using System.ComponentModel;

namespace LMS.Models
{
	public class Period
	{
		[DisplayName("Dag")]
		public int Day { get; set; }
		[DisplayName("Timstart")]
		public int StartHour { get; set; }
		[DisplayName("Minutstart")]
		public int StartMinute { get; set; }
		[DisplayName("Timslut")]
		public int EndHour { get; set; }
		[DisplayName("Minutslut")]
		public int EndMinute { get; set; }
		[DisplayName("Namn")]
		public string Name { get; set; }
		public int ModuleId { get; set; }
	}
}