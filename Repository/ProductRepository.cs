using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;

namespace MWBAYLY.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

    }
}
