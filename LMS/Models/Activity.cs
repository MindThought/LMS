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
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartTime { get; set; }
        [Required]
        public DateTime EndTime { get; set; }
        //public virtual List<Document> Documents { get; set; } TODO
    }
}