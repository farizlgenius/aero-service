using System;
using Aero.Application.DTOs;
using Aero.Domain.Entities;
using Aero.Domain.Enums;
using Aero.Domain.Interfaces;
using Aero.Infrastructure.Data;
using HID.Aero.ScpdNet.Wrapper;
using Microsoft.EntityFrameworkCore;

namespace Aero.Infrastructure.Repositories;

public sealed class ModuleRepository(AppDbContext context) : IBaseRepository<Module>
{
      public Task<int> AddAsync(Module data)
      {
            throw new NotImplementedException();
      }

      public Task<int> DeleteByComponentIdAsync(short component)
      {
            throw new NotImplementedException();
      }

      public Task<int> UpdateAsync(Module newData)
      {
            throw new NotImplementedException();
      }

      public async Task HandleFoundModuleAsync(SCPReplyMessage message)
        {
            if(message.tran.s_comm.comm_sts == 3)
            {
                if (await context.module.AsNoTracking().AnyAsync(x => x.serial_number.Equals(message.tran.s_comm.ser_num.ToString()))) return;
                var id = await helperService.GetLowestUnassignedNumberAsync<Module>(context, 16);
                if (id == 0) return;
                var mac = await helperService.GetMacFromIdAsync((short)message.SCPId);
                var module = new Module
                {
                    // Base 
                    component_id = id,
                    hardware_mac = mac,
                    location_id = await context.hardware.AsNoTracking().Where(x => x.mac.Equals(mac)).OrderBy(x => x.id).Select(x => x.location_id).FirstOrDefaultAsync(),
                    is_active = true,
                    created_date = DateTime.UtcNow,
                    updated_date = DateTime.UtcNow,

                    // extend_desc
                    model = message.tran.s_comm.model,
                    model_desc = Domain.Enums.Model.AeroX1100.ToString(),
                    revision = ((int)message.tran.s_comm.revision).ToString(),
                    address = -1,
                    port = 3,
                    n_input = (short)InputComponents.HIDAeroX1100,
                    n_output = (short)OutputComponents.HIDAeroX1100,
                    n_reader = (short)ReaderComponents.HIDAeroX1100,
                    msp1_no = 0,
                    baudrate = -1,
                    n_protocol = 0,
                    n_dialect = 0,

                };
            }else if(message.tran.s_comm.comm_sts == 2)
            {

            }
            
        }
}
