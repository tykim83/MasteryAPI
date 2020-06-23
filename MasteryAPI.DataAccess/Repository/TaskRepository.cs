using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class TaskRepository : Repository<Task>, ITaskRepository
    {
        private readonly DbContext context;

        public TaskRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}