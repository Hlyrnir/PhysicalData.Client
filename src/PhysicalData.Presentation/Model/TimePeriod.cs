using System;
using System.ComponentModel.DataAnnotations;

namespace PhysicalData.Presentation.Model
{
    public class TimePeriod
    {
        public string ConcurrencyStamp { get; init; } = string.Empty;

        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Example: [0.0, 0.1, 0.2, ...]")]
        public double[] Magnitude { get; set; } = Array.Empty<double>();

        [Range(double.MinValue, double.MaxValue, ErrorMessage = "Offset must be a numeric value.")]
        public double Offset { get; set; } = 0;

        [Required]
        public Guid PhysicalDimensionId { get; set; } = Guid.Empty;
    }
}
