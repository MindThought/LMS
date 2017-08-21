using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LMS.Models
{
    public enum ActivityType { ELearning, Lektion, Lab, Inlämning};

    public class Activity
    {
        public int Id { get; set; }
        public virtual Module Module { get; set; }
        [Required]
        public int ModuleId { get; set; }
        [DisplayName("Typ")]
        public ActivityType Type { get; set; }
        [Required]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [Required]
        [DisplayName("Starttid")]
        public DateTime StartTime { get; set; }
        [Required]
        [DisplayName("Sluttid")]
        public DateTime EndTime { get; set; }
        //public virtual List<Document> Documents { get; set; } TODO
    }
}