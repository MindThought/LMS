using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS.Models
{
    public class Module
    {
        [Key]
        public int Id { get; set; }
        [MaxLength(60)]
        [Required]
        [DisplayName("Modulnamn")]
        public string Name { get; set; }
        [DisplayName("Beskrivning")]
        public string Description { get; set; }
        [DisplayName("Starttid")]
        public DateTime? StartDate
        {
            get
            {
                var Start = Activities.OrderBy(a => a.StartTime).FirstOrDefault();
                if (Start == null)
                {
                    return DateTime.MinValue;
                }
                return Start.StartTime;
            }
        }
        [DisplayName("Sluttid")]
        public DateTime? EndDate
        {
            get
            {
                var End = Activities.OrderBy(a => a.EndTime).LastOrDefault();
                if (End == null)
                {
                    return DateTime.MinValue;
                }
                return End.EndTime;
            }
        }


        public virtual Course Course { get; set; }
        [Required]
        public int CourseId { get; set; }
        public virtual List<Activity> Activities { get; set; }
        //TODO public virtual Document Documents { get; set; } 
    }
}