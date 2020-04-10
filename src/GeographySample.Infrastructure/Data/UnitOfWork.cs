using GeographySample.Core;
using System.Threading.Tasks;

namespace GeographySample.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GeographyDbContext _context;

        public UnitOfWork(GeographyDbContext context)
        {
            _context = context;
        }

        public Task<int> CommitAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}