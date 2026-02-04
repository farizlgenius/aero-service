using System;
using Aero.Domain.Interfaces;
using HID.Aero.ScpdNet.Wrapper;

namespace Aero.Infrastructure.Adapter;

public sealed class TransactionAdaptor(SCPReplyMessage.SCPReplyTransaction tran) : ITransaction
{
      public int ser_num => tran.ser_num;

      public int time => tran.time;

      public short source_type => tran.source_type;

      public short source_number => tran.source_number;

      public short tran_type => tran.tran_type;

      public short tran_code => tran.tran_code;

      public ITypeSys sys => tran.sys is null ? null : new TypeSysAdapter(tran);

      public ITypeSysComm sys_comm => tran.sys_comm is null ? null : new  TypeSysCommAdapter(tran);

      public ITypeSioComm s_comm => tran.s_comm is null ? null : new  TypeSioCommAdapter(tran);

      public ITypeCardBin c_bin => tran.c_bin is null ? null : new TypeCardBin(tran);

      public ITypeCardBcd c_bcd => tran.c_bcd is null ? null : new TypeCardBcd(tran);

      public ITypeCardFull c_full => tran.c_full is null ? null : new TypeCardFull(tran);

      public ITypeCardID c_id => tran.c_id is null ? null : new TypeCardID(tran);

      public ITypeDblCardFull c_fulldbl => tran.c_fulldbl is null ? null : new TypeDblCardFull(tran);

      public ITypeDblCardID c_iddbl => throw new NotImplementedException();

      public ITypeI64CardFull c_fulli64 => throw new NotImplementedException();

      public ITypeI64CardFullIc32 c_fulli64i32 => throw new NotImplementedException();

      public ITypeHostCardFullPin c_fullHostPin => throw new NotImplementedException();

      public ITypeI64CardID c_idi64 => throw new NotImplementedException();

    public ITypeCoS cos => tran.cos is null ? null : new TypeCosAdapter(tran);

      public ITypeREX rex => throw new NotImplementedException();

      public ITypeCoSDoor door => throw new NotImplementedException();

      public ITypeProcedure proc => throw new NotImplementedException();

      public ITypeUserCmnd usrcmd => throw new NotImplementedException();

      public ITypeActivate act => throw new NotImplementedException();

      public ITypeAcr acr => throw new NotImplementedException();

      public ITypeMPG mpg => throw new NotImplementedException();

      public ITypeOAL oal => throw new NotImplementedException();

      public ITypeArea area => throw new NotImplementedException();

      public ITypeUseLimit c_uselimit => throw new NotImplementedException();

      public ITypeAsci t_diag => throw new NotImplementedException();

      public ITypeSioDiag s_diag => throw new NotImplementedException();

      public ITypeAcrExtFeatureStls extfeat_stls => throw new NotImplementedException();

      public ITypeAcrExtFeatureCoS extfeat_cos => throw new NotImplementedException();

      public ITypeWebActivity web_activity => throw new NotImplementedException();

      public ITypeOperatingMode oper_mode => throw new NotImplementedException();

      public ITypeCoSFloor floor => throw new NotImplementedException();

      public ITypeFileDownloadStatus file_download => throw new NotImplementedException();

      public ITypeCoSElevatorAccess elev_access => throw new NotImplementedException();

      public ITypeBatchReport batch_report => throw new NotImplementedException();
}
