using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class TaskDeleteDTO
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public int TaskId { get; set; }
    }
}