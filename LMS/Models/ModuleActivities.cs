using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMS.Models
{
    public class ModuleActivities
    {
        public int Id { get; set; }
        public List<Activity> ActivitySessions { get; set; }
        public List<Activity> FM { get; set; }
        public List<Activity> EM { get; set; }
        public List<string> Dates { get; set; }
        public List<DayOfWeek> WeekDays { get; set; }
        public Module Module { get; set; }
        public int ModuleId { get; set; }
    }
}