namespace LMS.Models
{
	public class Period
	{
		public int Day { get; set; }
		public int StartHour { get; set; }
		public int StartMinute { get; set; }
		public int EndHour { get; set; }
		public int EndMinute { get; set; }
		public string Name { get; set; }
		public int ModuleId { get; set; }
	}
}