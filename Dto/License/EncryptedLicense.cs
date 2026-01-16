using System;

namespace AeroService.DTO.License;

public record EncryptedLicense(
    string SessionId,
    string Payload,
    string Signature,
    string ServerSignPublic
);
