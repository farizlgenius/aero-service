using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interface
{
    public interface ICmndRepository : IBaseRepository<CommandAuditDto,CommandAudit>
    {
        Task<Pagination<CommandAudit>> GetCommandStatusAsync(PaginationParamsWithFilter pagination,short location);
    }
}
