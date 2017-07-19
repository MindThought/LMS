using System;
using System.Collections.Generic;

namespace LMS.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Course Course { get; set; }
        public virtual List<Activity> Activities { get; set; }
        //public virtual Document Documents { get; set; } TODO
    }
}