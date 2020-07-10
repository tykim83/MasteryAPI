using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Models
{
    public class RecordCreationCompleteBO
    {
        public DateTime Started { get; set; }

        public DateTime Finished { get; set; }

        public string Note { get; set; }

        public int CategoryId { get; set; }

        public int TaskId { get; set; }
        public string UserEmail { get; set; }
    }
}