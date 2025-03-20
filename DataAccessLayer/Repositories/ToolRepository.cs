using BusinessLayer.Entities;
using BusinessLayer.Interfaces;

namespace DataAccessLayer.Repositories
{
    public class ToolRepository(ApplicationDbContext context) : GenericRepository<Tool>(context), IToolRepository
    {
    }
}
