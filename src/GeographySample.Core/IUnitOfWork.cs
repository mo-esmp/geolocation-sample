using System.Threading.Tasks;

namespace GeographySample.Core
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync();
    }
}