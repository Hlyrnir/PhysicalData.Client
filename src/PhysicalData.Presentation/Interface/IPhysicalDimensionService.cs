using PhysicalData.Presentation.Filter;
using PhysicalData.Presentation.Model;
using PhysicalData.Presentation.Result;

namespace PhysicalData.Presentation.Interface
{
    public interface IPhysicalDimensionService
    {
        ValueTask<ApiResult<PagedResult<PhysicalDimension>>> FindPhysicalDimensionByFilter(PhysicalDimensionFilter fltPhysicalDimension, CancellationToken tknCancellation);
        ValueTask<ApiResult<PhysicalDimension>> FindPhysicalDimensionById(Guid guPhysicalDimensionId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryCreatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryDeletePhysicalDimension(Guid guPhysicalDimensionId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryUpdatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation);
    }
}