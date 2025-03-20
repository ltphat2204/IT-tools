using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IUserRepository Users { get; }
        public IGroupRepository Groups { get; }
        public IToolRepository Tools { get; }
        Task<int> SaveChangesAsync();
    }
}
