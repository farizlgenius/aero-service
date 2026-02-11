using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.Interfaces
{
    public interface IFileStorage
    {
        Task<string> SaveUserAsync(byte[] data, string fileName);
        Task<string> SaveMapAsync(byte[] data, string fileName);
        Task<Stream> ReadUserAsync(string fileName);
        Task<Stream> ReadMapAsync(string fileName);
        Task<string> ReadMapBase64Async(string fileName);
        Task<string> ReadUserBase64Async(string fileName);
        Task<string> SaveUserAsync(Stream stream, string fileName);
        void DeleteUserAsync(string filename);
        void DeleteMapAsync(string filename);
    }
}


