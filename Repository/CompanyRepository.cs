using MWBAYLY.Data;
using MWBAYLY.Models;
using MWBAYLY.Repository.IRepository;

namespace MWBAYLY.Repository
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext context;
        public CompanyRepository(ApplicationDbContext context) : base(context)
        {
            this.context = context;
        }
    }
}
