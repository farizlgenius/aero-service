using System;

namespace Aero.Application.Interfaces;

public interface ITransaction
{
      // Summary:
      //     serial number of this transaction
      int ser_num { get; }

      //
      // Summary:
      //     time of the transaction, seconds, 1970-based
      int time { get; }

      //
      // Summary:
      //     see the "tranSrc..." definitions
      short source_type { get; }

      //
      // Summary:
      //     defines the element of tranSrc
      short source_number { get; }

      //
      // Summary:
      //     see the "tranType..." definitions
      short tran_type { get; }

      //
      // Summary:
      //     defines the reason
      short tran_code { get; }

      //
      // Summary:
      //     system
      ITypeSys sys { get; }

      //
      // Summary:
      //     system with extended comm status fields
      ITypeSysComm sys_comm { get; }

      //
      // Summary:
      //     SIO communication status report
      ITypeSioComm s_comm { get; }

      //
      // Summary:
      //     binary card data
      ITypeCardBin c_bin { get; }

      //
      // Summary:
      //     card data
      ITypeCardBcd c_bcd { get; }

      //
      // Summary:
      //     formatted card: f/c, c/n, i/c
      ITypeCardFull c_full { get; }

      //
      // Summary:
      //     formatted card: card number only
      ITypeCardID c_id { get; }

      //
      // Summary:
      //     formatted card: f/c, c/n (double), i/c
      ITypeDblCardFull c_fulldbl { get; }

      //
      // Summary:
      //     formatted card: card number (double) only
      ITypeDblCardID c_iddbl { get; }

      //
      // Summary:
      //     formatted card: f/c, c/n (Int64), i/c
      ITypeI64CardFull c_fulli64 { get; }

      //
      // Summary:
      //     formatted card: f/c, c/n (Int64), i/c
      ITypeI64CardFullIc32 c_fulli64i32 { get; }

      //
      // Summary:
      //     formatted card: f/c, c/n (Int64), i/c, pin
      ITypeHostCardFullPin c_fullHostPin { get; }

      //
      // Summary:
      //     formatted card: card number (Int64) only
      ITypeI64CardID c_idi64 { get; }

      //
      // Summary:
      //     change-of-state
      ITypeCoS cos { get; }

      //
      // Summary:
      //     exit request
      ITypeREX rex { get; }

      //
      // Summary:
      //     Door Sts Monitor Change-Of-State
      ITypeCoSDoor door { get; }

      //
      // Summary:
      //     Procedure Control Command
      ITypeProcedure proc { get; }

      //
      // Summary:
      //     User Command Request report
      ITypeUserCmnd usrcmd { get; }

      //
      // Summary:
      //     COS TV tz ir triggers
      ITypeActivate act { get; }

      //
      // Summary:
      //     ACR mode
      ITypeAcr acr { get; }

      //
      // Summary:
      //     Monitor Point Group
      ITypeMPG mpg { get; }

      //
      // Summary:
      //     Offline Access List
      ITypeOAL oal { get; }

      //
      // Summary:
      //     Access Area
      ITypeArea area { get; }

      //
      // Summary:
      //     Use Limit update
      ITypeUseLimit c_uselimit { get; }

      //
      // Summary:
      //     ASCII diagnostic message
      ITypeAsci t_diag { get; }

      //
      // Summary:
      //     SIO comm diagnostics
      ITypeSioDiag s_diag { get; }

      //
      // Summary:
      //     Extended Feature Stateless Transaction
      ITypeAcrExtFeatureStls extfeat_stls { get; }

      //
      // Summary:
      //     Extended Feature Change-of-state
      ITypeAcrExtFeatureCoS extfeat_cos { get; }

      //
      // Summary:
      //     Web Activity transactions
      ITypeWebActivity web_activity { get; }

      //
      // Summary:
      //     Operating mode transactions
      ITypeOperatingMode oper_mode { get; }

      //
      // Summary:
      //     Elevator Floor CoS
      ITypeCoSFloor floor { get; }

      //
      // Summary:
      //     File Download Status
      ITypeFileDownloadStatus file_download { get; }

      //
      // Summary:
      //     Elevator Access
      ITypeCoSElevatorAccess elev_access { get; }

      //
      // Summary:
      //     Batch Report
      ITypeBatchReport batch_report { get; }
}

