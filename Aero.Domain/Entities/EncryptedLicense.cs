using System;

namespace Aero.Domain.Entities;

public record EncryptedLicense(
    string SessionId,
    string Payload,
    string Signature,
    string ServerSignPublic
);
