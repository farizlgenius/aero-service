using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Domain.Interfaces
{
    public interface IDbFunc<T>
    {
        void Update(T data);
        void ToggleStatus(bool status);
    }
}
