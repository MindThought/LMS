using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
	public class Period
	{
		public int day { get; set; }
		public int startHour { get; set; }
		public int startMinute { get; set; }
		public int endHour { get; set; }
		public int endMinute { get; set; }

		public int FillerFunction()
		{
			DateTime today = DateTime.Now;
			today.Subtract(new DateTime(0, 0, 0, today.Hour,today.Minute, today.Second));
			DateTime start = today;
			switch (today.DayOfWeek)
			{
				
				case DayOfWeek.Monday:

					break;
				case DayOfWeek.Tuesday:
					start.Subtract(new DateTime(0, 0, 1));
					break;
				case DayOfWeek.Wednesday:
					start.Subtract(new DateTime(0, 0, 2));
					break;
				case DayOfWeek.Thursday:
					start.Subtract(new DateTime(0, 0, 3));
					break;
				case DayOfWeek.Friday:
					start.Subtract(new DateTime(0, 0, 4));		
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
			context.Modules.Where(m => m.StartDate < today && m.EndDate > end).ToList();
			return 0;
		}
	}
}