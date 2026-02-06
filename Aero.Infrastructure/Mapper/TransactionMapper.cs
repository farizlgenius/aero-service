using System;

namespace Aero.Infrastructure.Mapper;

public sealed class TransactionMapper
{
      public static Aero.Infrastructure.Data.Entities.Transaction ToEf(Aero.Domain.Entities.Transaction data)
      {
            var res = new Aero.Infrastructure.Data.Entities.Transaction();
            MacBaseMapper.ToEf(data,res);
            res.date_time = data.DateTime;
            res.serial_number = data.SerialNumber;
            res.actor = data.Actor;
            res.source = data.Source;
            res.source_desc = data.SourceDesc;
            res.origin = data.Origin;
            res.source_module = data.SourceModule;
            res.type = data.Type;
            res.type_desc = data.TypeDesc;
            res.tran_code  =data.TranCode;
            res.image_path = data.Image;
            res.tran_code_desc  =data.TranCodeDesc;
            res.extend_desc = data.ExtendDesc;
            res.remark = data.Remark;
            res.hardware_mac = data.Mac;
            res.hardware_name = data.HardwareName;
            res.transaction_flag = data.TransactionFlags.Select(x => new Aero.Infrastructure.Data.Entities.TransactionFlag
            {
                  topic = x.Topic,
                  name = x.Name,
                  description = x.Description
            }).ToList();
            return res;
      }

}
