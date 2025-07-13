using MWBAYLY.Models;

namespace MWBAYLY.Repository.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        //object GetOne(Func<object, bool> value);
        object GetQueryable();
    }
}
