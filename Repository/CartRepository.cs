using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;

namespace MWBAYLY.Repository
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        private readonly ApplicationDbContext context;
        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
