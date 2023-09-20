using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;

namespace APIV2.Service.Repositories
{
    public class AddressRepository : GenericRepository<Address, GambleonContext>, IAddressRepository
    {
        public AddressRepository(GambleonContext context)
            : base(context)
        {

        }
    }
}
