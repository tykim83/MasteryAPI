using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly DbContext context;

        public CategoryRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}