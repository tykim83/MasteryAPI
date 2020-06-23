using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MasteryAPI.Models.DTOs
{
    public class RecordCreationStartDTO
    {
        public DateTime Started { get; set; }
        public string Note { get; set; }
    }
}