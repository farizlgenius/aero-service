using System;

namespace Aero.Application.Interfaces;

public interface ITypeAcr
{
      //
            // Summary:
            //     image of CC_ACR::actl_flags
             short actl_flags{ get; }

            //
            // Summary:
            //     flags prior to mode set
             short prior_flags{ get; }

            //
            // Summary:
            //     mode prior to mode set
             short prior_mode{ get; }

            //
            // Summary:
            //     image of CC_ACR::spare flags
             short actl_flags_e{ get; }

            //
            // Summary:
            //     prior image of CC_ACR::spare flags
             short prior_flags_e{ get; }

            //
            // Summary:
            //     current auth module flags
             int auth_mod_flags{ get; }

            //
            // Summary:
            //     prior auth module flags
             int prior_auth_mod_flags{ get; }
}
