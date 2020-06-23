using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class RecordDTO
    {
        public int Id { get; set; }

        public DateTime Started { get; set; }

        public DateTime? Finished { get; set; }
        public TimeSpan? TotalDuration { get; set; }
        public bool IsCompleted { get; set; }
        public string Note { get; set; }
    }
}