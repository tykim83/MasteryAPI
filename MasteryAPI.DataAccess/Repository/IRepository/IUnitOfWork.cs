using System;
using System.Collections.Generic;
using System.Text;

namespace MasteryAPI.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Account { get; }
        IRecordRepository Record { get; }

        void Save();
    }
}