using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class HolderCommandService : BaseAeroCommand, IHolderCommand
{

      public bool AccessDatabaseCardRecord(short ScpId, short Flags, long CardNumber, int IssueCode, string Pin, List<AccessLevelDto> AccessLevel, int Active, int Deactive = 2085970000)
      {
            CC_ADBC_I64DTIC32 cc = new CC_ADBC_I64DTIC32();
            cc.lastModified = 0;
            cc.scp_number = ScpId;
            cc.flags = Flags;
            cc.card_number = CardNumber;
            cc.issue_code = IssueCode;
            for (int i = 0; i < Pin.Length; i++)
            {
                  cc.pin[i] = Pin[i];
                  if (i == Pin.Length - 1)
                  {
                        cc.pin[i] = '\0';
                  }
            }
            for (int i = 0; i < AccessLevel.Count; i++)
            {
                  cc.alvl[i] = AccessLevel[i].component_id;
            }
            cc.act_time = Active;
            cc.dact_time = Deactive;

            bool flag = Send((short)enCfgCmnd.enCcAdbCardI64DTic32, cc);
            return flag;

      }

      public bool CardDelete(short ScpId, long CardNo)
      {
            CC_CARDDELETEI64 cc = new CC_CARDDELETEI64();
            cc.scp_number = ScpId;
            cc.cardholder_id = CardNo;

            bool flag = Send((short)enCfgCmnd.enCcCardDeleteI64, cc);
            return flag;
      }

}
