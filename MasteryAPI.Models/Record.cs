using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MasteryAPI.Models
{
    public class Record
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Started { get; set; }

        public DateTime? Finished { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task Task { get; set; }
    }
}