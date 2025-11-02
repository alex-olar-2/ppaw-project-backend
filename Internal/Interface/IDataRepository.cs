using Data.SDK.Repository.Interface;

namespace ExtractInfoIdentityDocument.Internal.Interface
{
    public interface IDataRepository<T> : IRepository<T> where T : class
    {
    }
}
