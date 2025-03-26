using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
{
    public class ToolRepository(ApplicationDbContext context) : GenericRepository<Tool>(context), IToolRepository
    {
        public IEnumerable<Tool> GetAllWithGroup()
        {
            return _context.Tools.Include(t => t.Group);
        }
    }
}
