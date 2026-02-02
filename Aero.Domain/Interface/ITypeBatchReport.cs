using System;

namespace Aero.Domain.Interfaces;

public interface ITypeBatchReport
{
      //
            // Summary:
            //     Trigger Number
             ushort triggerNumber{ get; }

            //
            // Summary:
            //     Batched Transactions
             uint activationCount{ get; }

            //
            // Summary:
            //     Source Type
             byte sourceType{ get; }

            //
            // Summary:
            //     Source Number
             ushort sourceNumber{ get; }

            //
            // Summary:
            //     Tran Type
             byte tranType{ get; }

            //
            // Summary:
            //     Tran Code Map (MSB First and LE bit order)
             byte[] tranCodeMap{ get; }
}
