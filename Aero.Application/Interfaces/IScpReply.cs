using System;
using Aero.Application.Interface;
using Aero.Application.Interfaces;
using Aero.Domain.Interface;

namespace Aero.Application.Interfaces;

public interface IScpReply
{
      int ScpId {get; }
      int ReplyType {get; }
      int TranType {get;}
      IIdreport id {get;}
      ICommStatus comm {get;}
      ISrSio sts_sio {get;}
      ISrMp sts_mp {get;}
      ISrCp sts_cp {get;}
      ISrAcr sts_acr {get;}
      IStrStatus str_sts {get;}
      ICcWebConfigNetwork web_network {get;}
      ICcWebConfigHostCommPrim web_host_comm_prim {get;}
      ITransaction tran {get;}
      ITranStatus tran_sts {get;}
        IScpReplyCmndStatus cmnd_sts { get; }

}

