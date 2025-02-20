using System.ComponentModel.DataAnnotations;

namespace PhysicalData.Presentation.Model
{
    public class PhysicalDimension
    {
        public string ConcurrencyStamp { get; init; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Conversion factor must be greater than 0.")]
        public double ConversionFactorToSI { get; set; } = 0.0;

        [Required(ErrorMessage = "Example: en-GB")]
        public string CultureName { get; set; } = "en-GB";

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfAmpere { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfCandela { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfKelvin { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfKilogram { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfMetre { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfMole { get; set; } = 0.0f;

        [Range(float.MinValue, float.MaxValue, ErrorMessage = "Exponent must be a numeric value.")]
        public float ExponentOfSecond { get; set; } = 0.0f;

        [Required]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "Example: Time")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Example: t")]
        public string Symbol { get; set; } = string.Empty;

        [Required(ErrorMessage = "Example: s")]
        public string Unit { get; set; } = string.Empty;
    }
}
