using Data.SDK;
using Data.SDK.Repository;

namespace ExtractInfoIdentityDocument.Internal
{
    public class Repository<T> : BaseRepository<T, DataContext> where T : class
    {
        public Repository(DataContext applicationDbContext)
            : base(applicationDbContext)
        {

        }
    }
}
