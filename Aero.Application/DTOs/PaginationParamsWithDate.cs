using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed class PaginationParamsWithDate
    {
        public int PageNumber { get; set; } = 1; // Default to page 1
        public int PageSize { get; set; } = 10; // Default page size
        public string Search { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
