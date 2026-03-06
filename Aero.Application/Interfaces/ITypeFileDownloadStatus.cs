using System;

namespace Aero.Application.Interfaces;

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
