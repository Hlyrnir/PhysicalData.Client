using PhysicalData.Contract.v01.Request;

namespace PhysicalData.Presentation.Filter
{
    public class PhysicalDimensionFilter : PagedRequest
    {
        public Guid? Id { get; set; } = null;
        public double? ConversionFactorToSI { get; set; } = null;
        public string? CultureName { get; set; } = null;
        public float? ExponentOfAmpere { get; set; } = null;
        public float? ExponentOfCandela { get; set; } = null;
        public float? ExponentOfKelvin { get; set; } = null;
        public float? ExponentOfKilogram { get; set; } = null;
        public float? ExponentOfMetre { get; set; } = null;
        public float? ExponentOfMole { get; set; } = null;
        public float? ExponentOfSecond { get; set; } = null;
        public string? Name { get; set; } = null;
        public string? Symbol { get; set; } = null;
        public string? Unit { get; set; } = null;
    }
}
