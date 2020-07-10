using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class TaskWithRecordsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public List<RecordOnlyDTO> Records { get; set; }
    }
}