using System;

namespace Aero.Application.DTOs;

public record EncryptedLicense(
    string SessionId,
    string Payload,
    string Signature,
    string ServerSignPublic
);
