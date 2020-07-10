using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Models
{
    public class TaskCreationBo
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string UserEmail { get; set; }
    }
}