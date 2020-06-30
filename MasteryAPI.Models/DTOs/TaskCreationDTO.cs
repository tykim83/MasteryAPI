using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class TaskCreationDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }
    }
}