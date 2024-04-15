﻿
namespace SyberSecurity.Interfaces
{
    public interface IUnitOfWork<T> : IDisposable where T : class
    {
        IGenericRepository<T> Entity { get; }

        Task CompleteAsync();
    }
}
