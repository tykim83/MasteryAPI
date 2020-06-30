using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Models
{
    public class RecordCreationStartBO
    {
        public string Note { get; set; }

        public int CategoryId { get; set; }

        public int TaskId { get; set; }
        public string UserEmail { get; set; }
    }
}