using AutoMapper;
using MasteryAPI.DataAccess.Repository.IRepository;
using MasteryAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db,
                            UserManager<ApplicationUser> userManager,
                            IConfiguration configuration,
                            SignInManager<ApplicationUser> signInManager,
                            IMapper mapper)
        {
            _db = db;
            Account = new AccountRepository(userManager, configuration, signInManager, mapper);
            Record = new RecordRepository(_db);
            Category = new CategoryRepository(_db);
            User = new UserRepository(_db);
        }

        public IAccountRepository Account { get; private set; }

        public IRecordRepository Record { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IUserRepository User { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}