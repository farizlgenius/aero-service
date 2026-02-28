using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface ICmndRepository : IBaseRepository<CommandAudit,CommandAudit,CommandAudit>
    {
        Task<Pagination<CommandAudit>> GetCommandStatusAsync(PaginationParamsWithFilter pagination,short location);
    }
}
