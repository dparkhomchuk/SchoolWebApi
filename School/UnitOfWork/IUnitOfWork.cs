using School.Repository;
using System;
using System.Threading.Tasks;

namespace School.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupRepository GroupRepository { get; }
        IStudentRepository StudentRepository { get; }
        Task Save();
    }
}
