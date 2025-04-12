using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class GroupRepository(ApplicationDbContext context) : GenericRepository<Group>(context), IGroupRepository
    {
        public IEnumerable<Group> GetAllWithTools()
        {
            return _context.Groups.Include(g => g.Tools);
        }
    }
}
