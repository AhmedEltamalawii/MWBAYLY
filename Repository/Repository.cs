using Microsoft.EntityFrameworkCore;
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;
using System.Linq.Expressions;

namespace MWBAYLY.Repository
{
    public class Repository<T>:IRepository<T> where T : class
    {
        private readonly ApplicationDbContext context;// this make injection
        DbSet<T> dbSet;//
        public Repository(ApplicationDbContext context)
        {
            this.context = context;
            dbSet=context .Set<T>(); // this alternative context.Categories this is use to storege My Type Genaric 
        }
        //public IEnumerable<T> GetAll(string? Include = null, Expression<Func<T, bool>>? expression = null)
       // OR
        public IEnumerable<T> GetAll(Expression<Func<T, Object>>? Include = null, Expression<Func<T, bool>>? expression = null) //  Expression<Func<T, bool>>? expression = null--> this is use to make falter Optional 

        {// IEnumerable --> not use with Include Bacause this use when I Stop work in database
            IQueryable query = dbSet; // IQueryable --> this ueing to build query and chack faltering in my database 
            return Include == null ? dbSet.ToList() : dbSet.Include(Include).ToList();
            if (Include != null)
            {
                return dbSet.Include(Include);// return dbSet.ToList();
            }

            return expression == null ? dbSet.ToList() : dbSet.Where(expression).ToList(); //return dbSet.Include(Include).ToList();


        }

        // IF I have Make Many of object You must make Arry 0-----> Expression<Func<T, Object>>[]? Include = null
        //public IEnumerable<T> GetAll(Expression<Func<T, Object>>[]? Include = null, Expression<Func<T, bool>>? expression = null) //  Expression<Func<T, bool>>? expression = null--> this is use to make falter Optional 

        //{// IEnumerable --> not use with Include Bacause this use when I Stop work in database
        //    IQueryable query = dbSet; // IQueryable --> this ueing to build query and chack faltering in my database 
        //    //  return Include == null ? dbSet.ToList() : dbSet.Include(Include).ToList();
        //    if (Include != null)
        //    {
        //        foreach (var item in Include) 
        //        {
        //             query=dbSet.Include(item);// return dbSet.ToList();
        //        }
        //    }

        //    return expression == null ? dbSet.ToList() : dbSet.Where(expression).ToList(); //return dbSet.Include(Include).ToList();


        //}
        //public T? GetOne(Expression<Func<T, bool>> expression, Expression<Func<T, Object>>? Include = null)
        //{
        //    //return GetAll(Include, expression).FirstOrDefault();
        //    return Include == null
        //      ? dbSet.AsNoTracking().FirstOrDefault(expression)
        //     : dbSet.Include(Include).AsNoTracking().FirstOrDefault(expression);
        //}
        public T? GetOne(Expression<Func<T,bool>>expression)
        {
            return dbSet.AsNoTracking().Where(expression).FirstOrDefault();
        }
        public void CreateNew(T entity)
        {
            dbSet.Add(entity);
            

        }
        public void Edit(T entity)
        {
            dbSet.Update(entity);

        }
        public void delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Commit()
        {
            context.SaveChanges();
        }

    }
}
