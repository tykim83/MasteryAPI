using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MasteryAPI.Utility
{
    public static class IEnumerableExensions
    {
        public static IEnumerable<T> Paginate<T>(this IEnumerable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}