using Aero.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Infrastructure.Mapper
{
    public sealed class CmndMapper
    {
        public static Aero.Infrastructure.Data.Entities.CommandAudit ToEf(Aero.Domain.Entities.CommandAudit data) 
        {
            return new Aero.Infrastructure.Data.Entities.CommandAudit
            {
                tag_no = data.TagNo,
                scp_id = data.ScpId,
                mac = data.Mac,
                command = data.Command,
                is_success = data.IsSuccess,
                is_pending = data.IsPending,
                nak_reason = data.NakReason,
                nake_desc_code = data.NakDescCode,
                location_id = data.LoationId,

                created_date = DateTime.UtcNow,
                updated_date = DateTime.UtcNow,
            };
        }

        public static void Update(Aero.Domain.Entities.CommandAudit from, Aero.Infrastructure.Data.Entities.CommandAudit to)
        {
            to.updated_date = DateTime.UtcNow;
            to.is_success = true;
            to.is_pending = false;
            to.nak_reason = from.NakReason;
            to.nake_desc_code = from.NakDescCode;
        }
    }
}
