using MWBAYLY.Models;
using System.Linq.Expressions;

namespace MWBAYLY.Repository.IRepository
{
    public interface IRepository <T> where T : class
    {
        // List<T> GetAll(string? Include = null); BN 
        // IEnumerable<T> GetAll(Expression<Func<T, Object>>? Include = null, Expression<Func<T, bool>>? expression = null, Func<object, object> include = null);
        public IEnumerable<T> GetAll(Expression<Func<T, Object>>? Include = null, Expression<Func<T, bool>>? expression = null);
       // T? GetOne(int categoryId);
        T? GetOne(Expression<Func<T, bool>> expression);
        void CreateNew(T category);

        void Edit(T category);

        void delete(T category);


        void Commit();
    }
}
