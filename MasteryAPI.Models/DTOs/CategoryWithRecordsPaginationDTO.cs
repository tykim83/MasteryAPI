using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class CategoryWithRecordsPaginationDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public List<RecordPaginationDTO> Records { get; set; }
        public double TotalAmountPages { get; set; }
    }
}