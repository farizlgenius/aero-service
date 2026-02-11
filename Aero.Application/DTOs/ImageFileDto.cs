using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs
{
    public sealed class ImageFileDto
    {
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public int FileSize { get; set; }
        public string FileData { get; set; } = string.Empty;
    }
}
