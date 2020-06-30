using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class CategoryWithTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TaskDTO> Tasks { get; set; }
    }
}