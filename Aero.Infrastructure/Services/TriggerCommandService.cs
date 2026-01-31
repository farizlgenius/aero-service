using System;
using Aero.Domain.Entities;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class TriggerCommandService : BaseAeroCommand,ITrigCommand
{
        #region Trigger

        public bool TriggerSpecification(short ScpId, Trigger data, short ComponentId)
        {
            CC_TRGR cc = new CC_TRGR();
            cc.scp_number = ScpId;
            cc.trgr_number = ComponentId;
            cc.command = data.Command;
            cc.proc_num = data.ProcedureId;
            cc.src_type = data.SourceType;
            cc.src_number = data.SourceNumber;
            cc.tran_type = data.TranType;
            foreach (var code in data.CodeMap)
            {
                cc.code_map += (int)Math.Pow(2, code.Value);
            }
            cc.timezone = data.TimeZone;
            switch (data.TranType)
            {
                case (short)tranType.tranTypeCardFull:
                    break;
                case (short)tranType.tranTypeCardID:
                    break;
                case (short)tranType.tranTypeCoS:
                    break;
                case (short)tranType.tranTypeCoSDoor:
                    break;
                case (short)tranType.tranTypeUserCmnd:
                    break;
                case (short)tranType.tranTypeOperatingMode:
                    break;
                case (short)tranType.tranTypeAcrExtFeatureCoS:
                    break;
                default:
                    break;
            }

            // trigger Variable
            //cc.trig_var[0] = 0;

            // transaction type_desc Arg
            //cc.arg[0] = 0;



            bool flag = Send((short)enCfgCmnd.enCcTrgr, cc);
            return flag;

        }

        #endregion
}
