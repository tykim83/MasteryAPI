using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(DbContext context)
            : base(context)
        {
            this.context = context;
        }
    }
}