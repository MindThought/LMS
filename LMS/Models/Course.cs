﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LMS.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate
        {
            get
            {
                var Order = Modules.OrderBy(a => a.StartDate);
                var Start = Order.Where(a => a.StartDate != DateTime.MinValue).FirstOrDefault();
                if (Start == null)
                {
                    return DateTime.MinValue;
                }
                return Start.StartDate;
            }
        }

        public virtual List<ApplicationUser> Students { get; set; }
        public virtual List<Module> Modules { get; set; }
        //public virtual List<Document> Documents { get; set; } TODO
    }
}