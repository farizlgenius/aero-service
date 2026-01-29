using System;
using Aero.Application.Interfaces;
using Aero.Domain.Entities;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Services;

public sealed class ScpCommandService : BaseAeroCommand, IScpCommand
{
      /// <summary>
      /// Command used for detach controller
      /// </summary>
      /// <param name="component">SCP Id</param>
      /// <returns>Command status</returns>
      public bool DetachScp(short component)
      {
            CC_ATTACHSCP c = new CC_ATTACHSCP();
            c.nSCPId = component;
            c.nChannelId = 0;
            bool flag = Send((short)enCfgCmnd.enCcDetachScp, c);
            return flag;
      }

      public bool ResetScp(short component)
      {
            CC_RESET cc_reset = new CC_RESET();
            cc_reset.scp_number = component;
            bool flag = Send((short)enCfgCmnd.enCcReset, cc_reset);
            return flag;
      }

      public bool ScpDeviceSpecification(short component, ScpSetting setting)
      {
            CC_SCP_SCP cc_scp_scp = new CC_SCP_SCP();
            cc_scp_scp.lastModified = 0;
            cc_scp_scp.number = component;
            cc_scp_scp.ser_num_low = 0;
            cc_scp_scp.ser_num_high = 0;
            cc_scp_scp.rev_major = 0;
            cc_scp_scp.rev_minor = 0;
            cc_scp_scp.nMsp1Port = setting.nMsp1Port;
            cc_scp_scp.nTransactions = setting.nTransaction;
            cc_scp_scp.nSio = setting.nSio;
            cc_scp_scp.nMp = setting.nMp;
            cc_scp_scp.nCp = setting.nCp;
            cc_scp_scp.nAcr = setting.nAcr;
            cc_scp_scp.nAlvl = setting.nAlvl;
            cc_scp_scp.nTrgr = setting.nTrgr;
            cc_scp_scp.nProc = setting.nProc;
            cc_scp_scp.gmt_offset = setting.gmtOffset;
            cc_scp_scp.nDstID = 0;
            cc_scp_scp.nTz = setting.nTz;
            cc_scp_scp.nHol = setting.nHol;
            cc_scp_scp.nMpg = setting.nMpg;
            cc_scp_scp.nTranLimit = 60000;
            cc_scp_scp.nAuthModType = 0;
            cc_scp_scp.nOperModes = 0;
            cc_scp_scp.oper_type = 1;
            cc_scp_scp.nLanguages = 0;
            cc_scp_scp.nSrvcType = 0;

            bool flag = Send((short)enCfgCmnd.enCcScpScp, cc_scp_scp);
            return flag;
      }

      public bool AccessDatabaseSpecification(short ScpId, ScpSetting setting)
      {
            CC_SCP_ADBS cc_scp_adbs = new CC_SCP_ADBS();
            cc_scp_adbs.lastModified = 0;
            cc_scp_adbs.nScpID = ScpId;
            cc_scp_adbs.nCards = setting.nCard;
            //cc_scp_adbs.nCards = 100;
            cc_scp_adbs.nAlvl = 32;
            // pin Constant = 1
            cc_scp_adbs.nPinDigits = 324;
            cc_scp_adbs.bIssueCode = 2;
            cc_scp_adbs.bApbLocation = 1;
            cc_scp_adbs.bActDate = 2;
            cc_scp_adbs.bDeactDate = 2;
            cc_scp_adbs.bVacationDate = 1;
            cc_scp_adbs.bUpgradeDate = 0;
            cc_scp_adbs.bUserLevel = 0;
            cc_scp_adbs.bUseLimit = 1;
            cc_scp_adbs.bSupportTimedApb = 1;
            cc_scp_adbs.nTz = 64;
            cc_scp_adbs.bAssetGroup = 0;
            cc_scp_adbs.nHostResponseTimeout = 5;
            cc_scp_adbs.nMxmTypeIndex = 0;
            cc_scp_adbs.nAlvlUse4Arq = 0;
            cc_scp_adbs.nFreeformBlockSize = 0;
            cc_scp_adbs.nEscortTimeout = 15;
            cc_scp_adbs.nMultiCardTimeout = 15;
            cc_scp_adbs.nAssetTimeout = 0;
            cc_scp_adbs.bAccExceptionList = 0;
            cc_scp_adbs.adbFlags = 1;

            bool flag = Send((short)enCfgCmnd.enCcScpAdbSpec, cc_scp_adbs);
            return flag;
      }

      public bool TimeSet(short component)
      {
            CC_TIME cc_time = new CC_TIME();
            cc_time.scp_number = component;
            cc_time.custom_time = 0;

            bool flag = Send((short)enCfgCmnd.enCcTime, cc_time);
            return flag;
      }

      public bool ReadStructureStatus(short component)
      {
            CC_STRSRQ cc_strsq = new CC_STRSRQ();
            cc_strsq.nScpID = component;
            cc_strsq.nListLength = 24;
            for (int i = 0; i < cc_strsq.nListLength; i++)
            {
                  switch (i)
                  {
                        case >= 15 and <= 19:
                              cc_strsq.nStructId[i] = (short)(i + 5);
                              break;
                        case 20:
                              cc_strsq.nStructId[i] = 26;
                              break;
                        case 21:
                              cc_strsq.nStructId[i] = 27;
                              break;
                        case 22:
                              cc_strsq.nStructId[i] = 33;
                              break;
                        case 23:
                              cc_strsq.nStructId[i] = 35;
                              break;
                        default:
                              cc_strsq.nStructId[i] = (short)(i + 1);
                              break;
                  }
            }

            bool flag = Send((short)enCfgCmnd.enCcStrSRq, cc_strsq);
            return flag;
      }

      public bool DeleteScp(short ScpId)
      {
            CC_NEWSCP c = new CC_NEWSCP();
            c.nSCPId = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcDeleteScp, c);
            return flag;

      }



      public bool SetScpId(short oldId, short newId)
      {
            CC_SCPID cc = new CC_SCPID();
            cc.scp_number = oldId;
            cc.scp_id = newId;
            bool flag = Send((short)enCfgCmnd.enCcScpID, cc);
            return flag;
      }

      public bool GetTransactionLogStatus(short ScpId)
      {
            CC_TRANSRQ c = new CC_TRANSRQ();
            c.scp_number = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcTranSrq, c);
            return flag;
      }

      public bool SetTransactionLogIndex(short ScpId, bool isEnable)
      {
            var _commandValue = (short)enCfgCmnd.enCcTranIndex;
            CC_TRANINDEX cc_tranindex = new CC_TRANINDEX();
            cc_tranindex.scp_number = ScpId;
            cc_tranindex.tran_index = isEnable ? -2 : -1;

            bool flag = Send(_commandValue, cc_tranindex);
            return flag;
      }

      public bool GetWebConfigRead(short ScpId, short type)
        {
            CC_WEB_CONFIG_READ cc = new CC_WEB_CONFIG_READ();
            cc.scp_number = ScpId;
            cc.read_type = type;

            bool flag = Send((short)enCfgCmnd.enCcWebConfigRead, cc);
            return flag;
        }

        public short CheckSCPStatus(short scpID)
        {
            return SCPDLL.scpCheckOnline(scpID);
        }

      public bool GetIdReport(short ScpId)
      {
            CC_IDREQUEST cc_idrequest = new CC_IDREQUEST();
            cc_idrequest.scp_number = ScpId;

            bool flag = Send((short)enCfgCmnd.enCcIDRequest, cc_idrequest);
            return flag;
      }



}
