using BusinessLayer.Entities;
using BusinessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class GroupRepository(ApplicationDbContext context) : GenericRepository<Group>(context), IGroupRepository
    {
    }
}
