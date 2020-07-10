using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Models
{
    public class TaskUpdateBO
    {
        public int CategoryId { get; set; }
        public int TaskId { get; set; }
        public string Name { get; set; }
        public string UserEmail { get; set; }
    }
}