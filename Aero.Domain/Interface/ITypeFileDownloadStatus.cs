using System;

namespace Aero.Domain.Interfaces;

public interface ITypeFileDownloadStatus
{
      //
            // Summary:
            //     File type
             byte fileType {get;}

            //
            // Summary:
            //     File Name
             char[] fileName {get;}
}
