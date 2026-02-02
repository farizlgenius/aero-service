using System;

namespace Aero.Domain.Interfaces;

public interface ISrAcr
{
      //
            // Summary:
            //     ACR number
             short number{get;}

            //
            // Summary:
            //     access control mode: C_308 encoded
             short mode{get;}

            //
            // Summary:
            //     reader tamper (TypeCoS::status)
             short rdr_status{get;}

            //
            // Summary:
            //     strike relay (TypeCoS::status)
             short strk_status{get;}

            //
            // Summary:
            //     door status map (TypeCoSDoor::door_status)
             short door_status{get;}

            //
            // Summary:
            //     access point status (TypeCoSDoor::ap_status)
             short ap_status{get;}

            //
            // Summary:
            //     rex-0 contact (TypeCoS::status)
             short rex_status0{get;}

            //
            // Summary:
            //     rex-1 contact (TypeCoS::status)
             short rex_status1{get;}

            //
            // Summary:
            //     reader led mode: C_315 encoded
             short led_mode{get;}

            //
            // Summary:
            //     acr config flags (CC_ACR::actl_flags)
             short actl_flags{get;}

            //
            // Summary:
            //     alternate reader tamper (TypeCoS::status)
             short altrdr_status{get;}

            //
            // Summary:
            //     extended flags (same as CC_ACR::spare)
             short actl_flags_extd{get;}

            //
            // Summary:
            //     Ext. Feature Type (0=None, 1=Classroom, 2=Office, 3=Privacy, 4=Apartment, ..)
             short nExtFeatureType{get;}

            //
            // Summary:
            //     Hardware Type in use.
             short nHardwareType{get;}

            //
            // Summary:
            //     Features variable by type, first byte hardware-specific binary inputs by convention.
             byte[] nExtFeatureStatus{get;}

            //
            // Summary:
            //     nAuthModFlags.
             int nAuthModFlags{get;}
}

