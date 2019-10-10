using School.Database.Repository;
using School.Repository;
using School.UnitOfWork;
using System.Threading.Tasks;

namespace School.Database.UnitOfWork
{
    public class EFUnitOfWork : IUnitOfWork
    {
        SchoolDataBase _context;
        IGroupRepository _groupRepository;
        IStudentRepository _studentRepository;
        public EFUnitOfWork(SchoolDataBase context)
        {
            _context = context;
        }
        public IGroupRepository GroupRepository
        {
            get
            {
                if (_groupRepository == null)
                    _groupRepository = new GroupRepository(_context);
                return _groupRepository;
            }
        }

        public IStudentRepository StudentRepository
        {
            get
            {
                if (_studentRepository == null)
                    _studentRepository = new StudentRepository(_context);
                return _studentRepository;
            }
        }



        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposedValue = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposedValue = true;
            }
        }
        public void Dispose()
        {
            Dispose(true);
        }
    }
}
