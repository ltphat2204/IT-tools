using BusinessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IToolRepository : IGenericRepository<Tool>
    {
        IEnumerable<Tool> GetAllWithGroup();
    }
}
