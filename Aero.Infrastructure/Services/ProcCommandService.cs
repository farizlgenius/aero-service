using System;
using Aero.Domain.Interface;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public class ProcCommandService : BaseAeroCommand, IProcCommand
{
      #region Procedure

      public bool ActionSpecificationAsyncForAllHW(short ComponentId, Aero.Domain.Entities.Action action, List<short> ScpIds)
      {
            foreach (var id in ScpIds)
            {
                  CC_ACTN c = new CC_ACTN(action.ActionType);
                  c.hdr.lastModified = 0;
                  c.hdr.scp_number = id;
                  c.hdr.proc_number = ComponentId;
                  c.hdr.action_type = action.ActionType;

                  switch (action.ActionType)
                  {
                        case 1:
                              c.mp_mask.scp_number = id;
                              c.mp_mask.mp_number = action.Arg1;
                              c.mp_mask.set_clear = action.Arg2;
                              break;
                        case 2:
                              c.cp_ctl.scp_number = id;
                              c.cp_ctl.cp_number = action.Arg1;
                              c.cp_ctl.command = action.Arg2;
                              c.cp_ctl.on_time = action.Arg3;
                              c.cp_ctl.off_time = action.Arg4;
                              c.cp_ctl.repeat = action.Arg5;
                              break;
                        case 3:
                              c.acr_mode.scp_number = id;
                              c.acr_mode.acr_number = action.Arg1;
                              c.acr_mode.acr_mode = action.Arg2;
                              break;
                        case 4:
                              c.fo_mask.scp_number = id;
                              c.fo_mask.acr_number = action.Arg1;
                              c.fo_mask.set_clear = action.Arg2;
                              break;
                        case 5:
                              c.ho_mask.scp_number = id;
                              c.ho_mask.acr_number = action.Arg1;
                              c.ho_mask.set_clear = action.Arg2;
                              break;
                        case 6:
                              c.unlock.scp_number = id;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 7:
                              c.proc.scp_number = id;
                              c.proc.proc_number = action.Arg1;
                              c.proc.command = action.Arg2;
                              break;
                        case 8:
                              c.tv_ctl.scp_number = id;
                              c.tv_ctl.tv_number = action.Arg1;
                              c.tv_ctl.set_clear = action.Arg2;
                              break;
                        case 9:
                              c.tz_ctl.scp_number = id;
                              c.tz_ctl.tz_number = action.Arg1;
                              c.tz_ctl.command = action.Arg2;
                              break;
                        case 10:
                              c.led_mode.scp_number = id;
                              c.led_mode.acr_number = action.Arg1;
                              c.led_mode.led_mode = action.Arg2;
                              break;
                        case 14:
                              c.mpg_set.scp_number = id;
                              c.mpg_set.mpg_number = action.Arg1;
                              c.mpg_set.command = action.Arg2;
                              c.mpg_set.arg1 = action.Arg3;
                              break;
                        case 15:
                              c.mpg_test_mask.scp_number = id;
                              c.mpg_test_mask.mpg_number = action.Arg1;
                              c.mpg_test_mask.action_prefix_ifz = action.Arg2;
                              c.mpg_test_mask.action_prefix_ifnz = action.Arg3;
                              break;
                        case 16:
                              c.mpg_test_active.scp_number = id;
                              c.mpg_test_active.mpg_number = action.Arg1;
                              c.mpg_test_active.action_prefix_ifnoactive = action.Arg2;
                              c.mpg_test_active.action_prefix_ifactive = action.Arg3;
                              break;
                        case 17:
                              c.area_set.scp_number = id;
                              c.area_set.area_number = action.Arg1;
                              c.area_set.command = action.Arg2;
                              c.area_set.occ_set = action.Arg3;
                              break;
                        case 18:
                              c.unlock.scp_number = id;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 19:
                              c.rled_tmp.scp_number = id;
                              c.rled_tmp.acr_number = action.Arg1;
                              c.rled_tmp.color_on = action.Arg2;
                              c.rled_tmp.color_off = action.Arg3;
                              c.rled_tmp.ticks_on = action.Arg4;
                              c.rled_tmp.ticks_off = action.Arg5;
                              c.rled_tmp.repeat = action.Arg6;
                              c.rled_tmp.beeps = action.Arg7;
                              break;
                        case 20:
                              //c.lcd_text.
                              break;
                        case 24:
                              c.temp_acr_mode.scp_number = id;
                              c.temp_acr_mode.acr_number = action.Arg1;
                              c.temp_acr_mode.acr_mode = action.Arg2;
                              c.temp_acr_mode.time = action.Arg3;
                              c.temp_acr_mode.nAuthModFlags = action.Arg4;
                              break;
                        case 25:
                              c.card_sim.nScp = id;
                              c.card_sim.nCommand = 1;
                              c.card_sim.nAcr = action.Arg1;
                              c.card_sim.e_time = action.Arg2;
                              c.card_sim.nFmtNum = action.Arg3;
                              c.card_sim.nFacilityCode = action.Arg4;
                              c.card_sim.nCardholderId = action.Arg5;
                              c.card_sim.nIssueCode = action.Arg6;
                              break;
                        case 26:
                              c.use_limit.scp_number = id;
                              c.use_limit.cardholder_id = action.Arg1;
                              c.use_limit.new_limit = action.Arg2;
                              break;
                        case 27:
                              c.oper_mode.scp_number = id;
                              c.oper_mode.oper_mode = action.Arg1;
                              c.oper_mode.enforce_existing = action.Arg2;
                              c.oper_mode.existing_mode = action.Arg3;
                              break;
                        case 28:
                              c.key_sim.nScp = id;
                              c.key_sim.nAcr = action.Arg1;
                              c.key_sim.e_time = action.Arg2;
                              for (int i = 0; i < action.StrArg.Length; i++)
                              {
                                    c.key_sim.keys[i] = action.StrArg[i];
                              }
                              break;
                        case 30:
                              c.batch_trans.nScpID = id;
                              c.batch_trans.trigNumber = (ushort)action.Arg1;
                              c.batch_trans.actType = (ushort)action.Arg2;
                              break;
                        case 126:
                        case 127:
                              c.delay.delay_time = action.Arg1;
                              break;
                        default:
                              break;
                  }

                  bool flag = Send((short)enCfgCmnd.enCcProc, c);
                  return flag;

            }

            return false;
      }

      public bool ActionSpecificationDelayAsync(short ComponentId, Aero.Domain.Entities.Action action)
      {

            CC_ACTN c = new CC_ACTN(127);
            c.hdr.lastModified = 0;
            c.hdr.scp_number = action.DeviceId;
            c.hdr.proc_number = ComponentId;
            c.hdr.action_type = 127;
            c.delay.delay_time = action.Arg1;
            //c.hdr.arg

            bool flag = Send((short)enCfgCmnd.enCcProc, c);
            return flag;
      }


      public bool ActionSpecificationAsync(short ComponentId, List<Aero.Domain.Entities.Action> en)
      {
            foreach (var action in en)
            {
                  if (action.ActionType == 9 || action.ActionType == 127) continue;
                  CC_ACTN c = new CC_ACTN(action.ActionType);
                  c.hdr.lastModified = 0;
                  c.hdr.scp_number = action.DeviceId;
                  c.hdr.proc_number = ComponentId;
                  c.hdr.action_type = action.ActionType;
                  //c.hdr.arg
                  switch (action.ActionType)
                  {
                        case 1:
                              c.mp_mask.scp_number = action.DeviceId;
                              c.mp_mask.mp_number = action.Arg1;
                              c.mp_mask.set_clear = action.Arg2;
                              break;
                        case 2:
                              c.cp_ctl.scp_number = action.DeviceId;
                              c.cp_ctl.cp_number = action.Arg1;
                              c.cp_ctl.command = action.Arg2;
                              c.cp_ctl.on_time = action.Arg3;
                              c.cp_ctl.off_time = action.Arg4;
                              c.cp_ctl.repeat = action.Arg5;
                              break;
                        case 3:
                              c.acr_mode.scp_number = action.DeviceId;
                              c.acr_mode.acr_number = action.Arg1;
                              c.acr_mode.acr_mode = action.Arg2;
                              break;
                        case 4:
                              c.fo_mask.scp_number = action.DeviceId;
                              c.fo_mask.acr_number = action.Arg1;
                              c.fo_mask.set_clear = action.Arg2;
                              break;
                        case 5:
                              c.ho_mask.scp_number = action.DeviceId;
                              c.ho_mask.acr_number = action.Arg1;
                              c.ho_mask.set_clear = action.Arg2;
                              break;
                        case 6:
                              c.unlock.scp_number = action.DeviceId;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 7:
                              c.proc.scp_number = action.DeviceId;
                              c.proc.proc_number = action.Arg1;
                              c.proc.command = action.Arg2;
                              break;
                        case 8:
                              c.tv_ctl.scp_number = action.DeviceId;
                              c.tv_ctl.tv_number = action.Arg1;
                              c.tv_ctl.set_clear = action.Arg2;
                              break;
                        case 9:
                              c.tz_ctl.scp_number = action.DeviceId;
                              c.tz_ctl.tz_number = action.Arg1;
                              c.tz_ctl.command = action.Arg2;
                              break;
                        case 10:
                              c.led_mode.scp_number = action.DeviceId;
                              c.led_mode.acr_number = action.Arg1;
                              c.led_mode.led_mode = action.Arg2;
                              break;
                        case 14:
                              c.mpg_set.scp_number = action.DeviceId;
                              c.mpg_set.mpg_number = action.Arg1;
                              c.mpg_set.command = action.Arg2;
                              c.mpg_set.arg1 = action.Arg3;
                              break;
                        case 15:
                              c.mpg_test_mask.scp_number = action.DeviceId;
                              c.mpg_test_mask.mpg_number = action.Arg1;
                              c.mpg_test_mask.action_prefix_ifz = action.Arg2;
                              c.mpg_test_mask.action_prefix_ifnz = action.Arg3;
                              break;
                        case 16:
                              c.mpg_test_active.scp_number = action.DeviceId;
                              c.mpg_test_active.mpg_number = action.Arg1;
                              c.mpg_test_active.action_prefix_ifnoactive = action.Arg2;
                              c.mpg_test_active.action_prefix_ifactive = action.Arg3;
                              break;
                        case 17:
                              c.area_set.scp_number = action.DeviceId;
                              c.area_set.area_number = action.Arg1;
                              c.area_set.command = action.Arg2;
                              c.area_set.occ_set = action.Arg3;
                              break;
                        case 18:
                              c.unlock.scp_number = action.DeviceId;
                              c.unlock.acr_number = action.Arg1;
                              c.unlock.floor_number = action.Arg2;
                              c.unlock.strk_tm = action.Arg3;
                              c.unlock.t_held = action.Arg4;
                              c.unlock.t_held_pre = action.Arg5;
                              break;
                        case 19:
                              c.rled_tmp.scp_number = action.DeviceId;
                              c.rled_tmp.acr_number = action.Arg1;
                              c.rled_tmp.color_on = action.Arg2;
                              c.rled_tmp.color_off = action.Arg3;
                              c.rled_tmp.ticks_on = action.Arg4;
                              c.rled_tmp.ticks_off = action.Arg5;
                              c.rled_tmp.repeat = action.Arg6;
                              c.rled_tmp.beeps = action.Arg7;
                              break;
                        case 20:
                              //c.lcd_text.
                              break;
                        case 24:
                              c.temp_acr_mode.scp_number = action.DeviceId;
                              c.temp_acr_mode.acr_number = action.Arg1;
                              c.temp_acr_mode.acr_mode = action.Arg2;
                              c.temp_acr_mode.time = action.Arg3;
                              c.temp_acr_mode.nAuthModFlags = action.Arg4;
                              break;
                        case 25:
                              c.card_sim.nScp = action.DeviceId;
                              c.card_sim.nCommand = 1;
                              c.card_sim.nAcr = action.Arg1;
                              c.card_sim.e_time = action.Arg2;
                              c.card_sim.nFmtNum = action.Arg3;
                              c.card_sim.nFacilityCode = action.Arg4;
                              c.card_sim.nCardholderId = action.Arg5;
                              c.card_sim.nIssueCode = action.Arg6;
                              break;
                        case 26:
                              c.use_limit.scp_number = action.DeviceId;
                              c.use_limit.cardholder_id = action.Arg1;
                              c.use_limit.new_limit = action.Arg2;
                              break;
                        case 27:
                              c.oper_mode.scp_number = action.DeviceId;
                              c.oper_mode.oper_mode = action.Arg1;
                              c.oper_mode.enforce_existing = action.Arg2;
                              c.oper_mode.existing_mode = action.Arg3;
                              break;
                        case 28:
                              c.key_sim.nScp = action.DeviceId;
                              c.key_sim.nAcr = action.Arg1;
                              c.key_sim.e_time = action.Arg2;
                              for (int i = 0; i < action.StrArg.Length; i++)
                              {
                                    c.key_sim.keys[i] = action.StrArg[i];
                              }
                              break;
                        case 30:
                              c.batch_trans.nScpID = action.DeviceId;
                              c.batch_trans.trigNumber = (ushort)action.Arg1;
                              c.batch_trans.actType = (ushort)action.Arg2;
                              break;
                        case 126:
                        case 127:
                              c.delay.delay_time = action.Arg1;
                              break;
                        default:
                              break;
                  }

                  bool flag = Send((short)enCfgCmnd.enCcProc, c);
                  return flag;

            }
            return false;
      }


      #endregion
}
