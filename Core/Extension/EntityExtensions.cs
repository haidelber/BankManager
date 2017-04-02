using BankManager.Data.Entity;

namespace BankManager.Core.Extension
{
    public static class EntityExtensions
    {
        public static bool HasValidId<T>(this T entity) where T : EntityBase
        {
            return entity.Id != default(long);
        }
    }
}
