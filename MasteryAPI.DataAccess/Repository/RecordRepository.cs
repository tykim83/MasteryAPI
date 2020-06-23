using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class RecordRepository : Repository<Record>, IRecordRepository
    {
        private readonly DbContext context;

        public RecordRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}