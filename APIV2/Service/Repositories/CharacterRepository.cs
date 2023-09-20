using APIV2.Data;
using APIV2.Models;
using APIV2.Service.Interfaces;

namespace APIV2.Service.Repositories
{
    public class CharacterRepository : GenericRepository<Character, GambleonContext>, ICharacterRepository
    {
        public CharacterRepository(GambleonContext context)
            : base(context)
        {

        }
    }
}
