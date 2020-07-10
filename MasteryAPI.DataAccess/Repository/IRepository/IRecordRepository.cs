using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MasteryAPI.DataAccess.Repository.IRepository
{
    public interface IRecordRepository : IRepository<Record>
    {
        IQueryable<Record> GetQueryable(Expression<Func<Record, bool>> filter = null, Func<IQueryable<Record>, IOrderedQueryable<Record>> orderBy = null, string includeProperties = null);
    }
}