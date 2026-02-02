using System;

namespace Aero.Domain.Interfaces;

public interface ITypeAcrExtFeatureStls
{
 //
            // Summary:
            //     Ext. Feature Type (0=None, ...)
             short nExtFeatureType {get;}

            //
            // Summary:
            //     Hardware Type in Use.
             short nHardwareType  {get;}

            //
            // Summary:
            //     Associated Data (by feature type)
             byte[] nExtFeatureData  {get;}

            //
            // Summary:
            //     Extended Feature Status
             byte[] nExtFeatureStatus  {get;}
}
