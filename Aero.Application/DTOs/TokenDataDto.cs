using System;

namespace Aero.Application.DTOs;

public sealed record TokenDataDto(TokenUserDataDto User,List<int> Locations,TokenRoleData Role );

public sealed record TokenUserDataDto(string Username,string Title,string Firstname,string Middlename,string Lastname);

public sealed record TokenRoleData (int Id,string Name,List<int> Features);
