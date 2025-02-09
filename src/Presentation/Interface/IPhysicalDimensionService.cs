using Presentation.Filter;
using Presentation.Model;
using Presentation.Result;

namespace Presentation.Interface
{
    public interface IPhysicalDimensionService
    {
        ValueTask<ApiResult<IEnumerable<PhysicalDimension>>> FindPhysicalDimensionByFilter(PhysicalDimensionFilter fltPhysicalDimension, CancellationToken tknCancellation);
        ValueTask<ApiResult<PhysicalDimension>> FindPhysicalDimensionById(Guid guPhysicalDimensionId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryCreatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryDeletePhysicalDimension(Guid guPhysicalDimensionId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryUpdatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation);
    }
}