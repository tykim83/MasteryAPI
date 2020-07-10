﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class CategoryWithTaskDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public TimeSpan TotalDuration { get; set; }
        public List<TaskWithRecordsDTO> Tasks { get; set; }
    }
}