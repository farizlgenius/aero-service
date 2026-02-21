using System;

namespace Aero.Application.Interfaces;

public interface ITypeAcrExtFeatureCoS
{
      //
      // Summary:
      //     Ext. Feature Type (0=None, ...)
      short nExtFeatureType { get; }

      //
      // Summary:
      //     Hardware Type in Use.
      short nHardwareType { get; }

      //
      // Summary:
      //     Point (0=Deadbolt, etc.)
      short nExtFeaturePoint { get; }

      //
      // Summary:
      //     Current Status (trl07 encoded)
      byte nStatus { get; }

      //
      // Summary:
      //     Prior Status (trl07 encoded)
      byte nStatusPrior { get; }

      //
      // Summary:
      //     Associated Data (by feature type)
      byte[] nExtFeatureData { get; }

      //
      // Summary:
      //     Extended Feature Status

      byte[] nExtFeatureStatus { get; }
}
