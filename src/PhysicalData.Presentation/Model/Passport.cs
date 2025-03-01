using PassportCheckpoint.Interface;
using System;
using System.Collections.Generic;

namespace PhysicalData.Presentation.Model
{
    public sealed class Passport : IPassport
    {
        public required string EmailAddress { get; init; }
        public required DateTimeOffset ExpiredAt { get; init; }
        public required Guid HolderId { get; init; }
        public required bool IsAuthority { get; init; }
        public required bool IsEnabled { get; init; }
        public required IEnumerable<IPassportVisa> Visa { get; init; }
    }
}
