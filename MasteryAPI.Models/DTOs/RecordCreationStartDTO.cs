﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class RecordCreationStartDTO
    {
        public string Note { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public int TaskId { get; set; }
    }
}