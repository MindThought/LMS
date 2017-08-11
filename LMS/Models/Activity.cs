using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public enum ActivityType { ELearning, Lecture, Lab, Submission};

    public class Activity
    {
        public int Id { get; set; }
        public virtual Module Module { get; set; }
        [Required]
        public int ModuleId { get; set; }
        public ActivityType Type { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public virtual List<Document> Documents { get; set; } TODO
    }
}