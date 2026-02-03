using System;
using Aero.Application.DTOs;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class DoorCommandService : BaseAeroCommand, IDoorCommand
{
      public bool ReaderSpecification(short ScpId, short SioNo, short ReaderNo, short DataFormat, short KeyPadMode, short LedDriveMode, short OsdpFlag)
      {
            CC_RDR cc_rdr = new CC_RDR();
            cc_rdr.lastModified = 0;
            cc_rdr.scp_number = ScpId;
            cc_rdr.sio_number = SioNo;
            cc_rdr.reader = ReaderNo;
            cc_rdr.dt_fmt = DataFormat;
            cc_rdr.keypad_mode = KeyPadMode;
            cc_rdr.led_drive_mode = LedDriveMode;
            cc_rdr.osdp_flags = OsdpFlag;
            //cc_rdr.device_id

            bool flag = Send((short)enCfgCmnd.enCcReader, cc_rdr);
            return flag;
      }

      public bool AccessControlReaderConfiguration(short ScpId, short AcrNo, Door dto)
      {
            CC_ACR cc_acr = new CC_ACR();
            cc_acr.lastModified = 0;
            cc_acr.scp_number = ScpId;
            cc_acr.acr_number = AcrNo;
            cc_acr.access_cfg = dto.AccessConfig;
            cc_acr.pair_acr_number = dto.PairDoorNo;
            cc_acr.rdr_sio = dto.Readers.ElementAt(0).ModuleId;
            cc_acr.rdr_number = dto.Readers.ElementAt(0).ModuleId;
            cc_acr.strk_sio = dto.Strk.ModuleId;
            cc_acr.strk_number = dto.Strk.OutputNo;
            cc_acr.strike_t_min = dto.Strk.StrkMin;
            cc_acr.strike_t_max = dto.Strk.StrkMax;
            cc_acr.strike_mode = dto.Strk.StrkMode;
            cc_acr.door_sio = dto.Sensor.ModuleId;
            cc_acr.door_number = dto.Sensor.InputNo;
            cc_acr.dc_held = dto.Sensor.DcHeld;
            if (dto.RequestExits is not null)
            {
                  cc_acr.rex0_sio = dto.RequestExits.ElementAt(0).ModuleId;
                  cc_acr.rex0_number = dto.RequestExits.ElementAt(0).InputNo;
                  cc_acr.rex_tzmask[0] = dto.RequestExits.ElementAt(0).MaskTimeZone;
                  if (dto.RequestExits.Count > 1)
                  {
                        cc_acr.rex1_sio = dto.RequestExits.ElementAt(1).ModuleId;
                        cc_acr.rex1_number = dto.RequestExits.ElementAt(1).InputNo;
                        cc_acr.rex_tzmask[1] = dto.RequestExits.ElementAt(1).MaskTimeZone;
                  }
            }
            if (dto.Readers.Count > 1)
            {
                  cc_acr.altrdr_sio = dto.Readers.ElementAt(1).ModuleId;
                  cc_acr.altrdr_number = dto.Readers.ElementAt(1).ReaderNo;
                  cc_acr.altrdr_spec = dto.ReaderOutConfiguration;
            }
            cc_acr.cd_format = dto.CardFormat;
            cc_acr.apb_mode = dto.AntiPassbackMode;
            if (dto.AntiPassBackIn > 0) cc_acr.apb_in = (short)dto.AntiPassBackIn;
            if (dto.AntiPassBackOut > 0) cc_acr.apb_to = (short)dto.AntiPassBackOut!;
            if (dto.SpareTags != -1) cc_acr.spare = dto.SpareTags;
            if (dto.AccessControlFlags != -1) cc_acr.actl_flags = dto.AccessControlFlags;
            cc_acr.offline_mode = dto.OfflineMode;
            cc_acr.default_mode = dto.DefaultMode;
            cc_acr.default_led_mode = dto.DefaultLEDMode;
            cc_acr.pre_alarm = dto.PreAlarm;
            cc_acr.apb_delay = dto.AntiPassbackDelay;
            cc_acr.strk_t2 = dto.StrkT2;
            cc_acr.dc_held2 = dto.DcHeld2;
            cc_acr.strk_follow_pulse = 0;
            cc_acr.strk_follow_delay = 0;
            cc_acr.nAuthModFlags = 0;
            cc_acr.nExtFeatureType = 0;
            cc_acr.dfofFilterTime = 0;

            bool flag = Send((short)enCfgCmnd.enCcACR, cc_acr);
            return flag;

      }

      public bool MomentaryUnlock(short ScpId, short AcrNo)
        {
            CC_UNLOCK cc_unlock = new CC_UNLOCK();
            cc_unlock.scp_number = ScpId;
            cc_unlock.acr_number = AcrNo;

            bool flag = Send((short)enCfgCmnd.enCcUnlock, cc_unlock);
            return flag;

        }

        public bool GetAcrStatus(short ScpId, short AcrNo, short Count)
        {
            CC_ACRSRQ cc = new CC_ACRSRQ();
            cc.scp_number = ScpId;
            cc.first = AcrNo;
            cc.count = Count;

            bool flag = Send((short)enCfgCmnd.enCcAcrSrq, cc);
            return flag;
        }

        public bool AcrMode(short ScpId, short AcrNo, short Mode)
        {
            CC_ACRMODE cc = new CC_ACRMODE();
            cc.scp_number = ScpId;
            cc.acr_number = AcrNo;
            cc.acr_mode = Mode;
            cc.nAuthModFlags = 0;
            //cc.n_ext_feature_type

            bool flag = Send((short)enCfgCmnd.enCcAcrMode, cc);
            return flag;
        }
}
