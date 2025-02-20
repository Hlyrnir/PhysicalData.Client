using PhysicalData.Contract.v01.Request;

namespace PhysicalData.Presentation.Filter
{
    public class TimePeriodFilter : PagedRequest
    {
        public Guid? Id { get; set; } = null;
        public string? Magnitude { get; set; } = null;
        public double? Offset { get; set; } = null;
        public Guid? PhysicalDimensionId { get; set; } = null;
    }
}
