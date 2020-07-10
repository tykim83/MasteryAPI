using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using MasteryAPI.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        private readonly ApplicationDbContext dbContext;

        public RecordRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Record> GetQueryable(Expression<Func<Record, bool>> filter = null, Func<IQueryable<Record>, IOrderedQueryable<Record>> orderBy = null, string includeProperties = null)
        {
            IQueryable<Record> query = dbContext.Records;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            //Include properties will be separated by a coma
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query);
            }

            return query;
        }
    }
}