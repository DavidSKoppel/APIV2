using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;

namespace APIV2.Service.Repositories
{
    public class UserTypeRepository : GenericRepository<UserType, GambleonContext>, IUserTypeRepository
    {
        public UserTypeRepository(GambleonContext context)
            : base(context)
        {

        }
    }
}
