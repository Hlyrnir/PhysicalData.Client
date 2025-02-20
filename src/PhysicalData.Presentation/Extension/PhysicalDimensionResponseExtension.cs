using PhysicalData.Contract.v01.Response.PhysicalDimension;
using PhysicalData.Presentation.Model;

namespace PhysicalData.Presentation.Extension
{
    internal static class PhysicalDimensionResponseExtension
    {
        internal static PhysicalDimension ParseToModel(this PhysicalDimensionByIdResponse rspnPhysicalDimension)
        {
            return new PhysicalDimension()
            {
                ConcurrencyStamp = rspnPhysicalDimension.ConcurrencyStamp,
                ConversionFactorToSI = rspnPhysicalDimension.ConversionFactorToSI,
                CultureName = rspnPhysicalDimension.CultureName,
                ExponentOfAmpere = rspnPhysicalDimension.ExponentOfAmpere,
                ExponentOfCandela = rspnPhysicalDimension.ExponentOfCandela,
                ExponentOfKelvin = rspnPhysicalDimension.ExponentOfKelvin,
                ExponentOfKilogram = rspnPhysicalDimension.ExponentOfKilogram,
                ExponentOfMetre = rspnPhysicalDimension.ExponentOfMetre,
                ExponentOfMole = rspnPhysicalDimension.ExponentOfMole,
                ExponentOfSecond = rspnPhysicalDimension.ExponentOfSecond,
                Id = rspnPhysicalDimension.Id,
                Name = rspnPhysicalDimension.Name,
                Symbol = rspnPhysicalDimension.Symbol,
                Unit = rspnPhysicalDimension.Unit
            };
        }

        internal static IEnumerable<PhysicalDimension> ParseToModel(this IEnumerable<PhysicalDimensionByIdResponse> enumPhysicalDimension)
        {
            foreach (PhysicalDimensionByIdResponse rspnPhysicalDimension in enumPhysicalDimension)
            {
                yield return rspnPhysicalDimension.ParseToModel();
            }
        }
    }
}
