using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasteryAPI.Models
{
    public class Record
    {
        public int Id { get; set; }

        [Required]
        public DateTime Started { get; set; }

        public DateTime? Finished { get; set; }
        public TimeSpan? TotalDuration { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }
    }
}