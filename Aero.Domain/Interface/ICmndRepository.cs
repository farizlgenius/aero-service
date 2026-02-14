using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Interface
{
    public interface ICmndRepository : IBaseRepository<CommandAudit>
    {
        Task<Pagination<CommandAudit>> GetCommandStatusAsync(PaginationParamsWithFilter pagination,short location);
    }
}
