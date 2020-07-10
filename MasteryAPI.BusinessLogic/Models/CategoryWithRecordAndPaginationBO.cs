using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.BusinessLogic.Models
{
    public class CategoryWithRecordAndPaginationBO
    {
        public int CategoryId { get; set; }
        public string UserEmail { get; set; }
        public PaginationDTO PaginationDTO { get; set; }
    }
}