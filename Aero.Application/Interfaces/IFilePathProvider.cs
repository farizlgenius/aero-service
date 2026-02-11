using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IFilePathProvider
    {
        string Users { get; }
        string Maps { get; }
    }
}
