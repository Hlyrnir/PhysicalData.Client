using PassportCheckpoint.Interface;

namespace Presentation.Model
{
    public sealed class PassportVisa : IPassportVisa
    {
        public required int Level { get; init; }
        public required string Name { get; init; }
    }
}
