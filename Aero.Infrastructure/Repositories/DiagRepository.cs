using Aero.Application.Interface;
using Aero.Domain.Interface;
using Aero.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Repositories
{
    public sealed class DiagRepository(BaseAeroCommand com) : IDiagRepository
    {
        public bool CommandAsync(string command)
        {
           return com.SendASCIICommand(command);
        }
    }
}
