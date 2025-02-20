using PassportCheckpoint.Interface;

namespace PhysicalData.Presentation.Model
{
    public sealed class PassportCredential : IPassportCredential
    {
        public required string Credential { get; set; }
        public required string Signature { get; set; }
    }
}