using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aero.Application.DTOs;

public sealed record ImageFileDto(string FileName, string ContentType, int FileSize, string FileData);
