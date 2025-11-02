using Data.SDK.Repository;

using ExtractInfoIdentityDocument.Internal.Interface;

namespace ExtractInfoIdentityDocument.Internal
{
    public class DataRepository<T> : Repository<T, DataContext>, IDataRepository<T> where T : class
    {
        private readonly DataContext _dataContext;

        public DataRepository(DataContext dataContext)
            : base(dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<int> InsertSqlAsync(string sql)
        {
            return await _dataContext.ExecuteSqlRawAsync(sql, 5000);
        }

    }
}
