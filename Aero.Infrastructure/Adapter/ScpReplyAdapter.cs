using System;
using Aero.Domain.Interface;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public class ScpReplyAdapter(SCPReplyMessage message) : IScpReply
{
      
      public int ScpId => message.SCPId;
      public int ReplyType => message.ReplyType;
      public int TranType => message.tran != null ?  message.tran.tran_type : 0;
      public IIdreport id => message.id is null ? null : new IdReportAdapter(message.id);
      public ICommStatus comm => message.comm is null ? null : new CommStatusAdapter(message.comm);
      public ISrSio sts_sio => message.sts_sio is null ? null : new SrSioAdapter(message.sts_sio);
      public ISrMp sts_mp => message.sts_mp is null ? null : new SrMpAdapter(message.sts_mp);
      public ISrCp sts_cp => message.sts_cp is null ? null : new SrCpAdapter(message.sts_cp);
      public ISrAcr sts_acr => message.sts_acr is null ? null : new SrAcrAdapter(message.sts_acr);
      public IStrStatus str_sts => message.str_sts is null ? null : new StrStatusAdapter(message.str_sts);
      public ICcWebConfigNetwork web_network => message.web_network is null ? null : new CcWebConfigNetwork(message.web_network);
      public ICcWebConfigHostCommPrim web_host_comm_prim => message.web_host_comm_prim is null ? null : new CcWebConfigHostCommPrim(message.web_host_comm_prim);
      public ITransaction tran => message.tran is null ? null : new TransactionAdaptor(message.tran);
      public ITranStatus tran_sts => message.tran_sts is null ? null : new TranStatusAdapter(message.tran_sts);

    public IScpReplyCmndStatus cmnd_sts => message.cmnd_sts is null ? null : new ScpReplyCmndStatusAdapter(message.cmnd_sts);
}
