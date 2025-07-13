
using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace MWBAYLY.Repository
{
    public class CategoryRepository: Repository<Category> , ICategoryRepository
    {
        // ApplicationDbContext context =new ApplicationDbContext();
       private readonly ApplicationDbContext context;
        public CategoryRepository(ApplicationDbContext context):base(context)
        {
            this.context = context; 
        }

        object ICategoryRepository.GetQueryable()
        {
            throw new NotImplementedException();
        }

        //public object GetQueryable()
        //{
        //    throw new NotImplementedException();
        //}


        /////////////////////////////////// -- this block replace in genaric Repository -- /////////////////////////////
        //public object GetOne(Func<object, bool> value)
        //{
        //    throw new NotImplementedException();
        //}
        //public List<Category> GetAll(string? Include = null)

        //{
        //    //return Include==null ? context.categories.ToList(): context.categories.Include(Include).ToList();
        //    if (Include == null)
        //    {
        //        return context.categories.ToList();
        //    }
        //    {
        //        return context.categories.Include(Include).ToList();
        //    }

        //}
        //public Category? GetOne(int categoryId)
        //{
        //    return context.categories.FirstOrDefault(e => e.Id == categoryId);
        //}
        //public void CreateNew(Category category)
        //{
        //    context.categories.Add(category);

        //}
        //public void Edit(Category category)
        //{
        //    context.categories.Update(category);

        //}
        //public void delete(Category category)
        //{
        //    context.categories.Remove(category);

        //}

        //public void Commit()
        //{
        //    context.SaveChanges();
        //}
    }
}
