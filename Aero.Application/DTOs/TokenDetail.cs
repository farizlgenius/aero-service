namespace Aero.Application.DTOs;

public sealed record TokenDetail(bool Auth, TokenInfo? Info);

public sealed record TokenInfo(Users User, List<short> Locations, Role Role);

public sealed record Users(string Title, string Firstname, string Middlename, string Lastname, string Email);

public sealed record Location(short LocationNo, string LocationName);

public sealed record Role(short RoleNo, string RoleName, List<short> Features);
