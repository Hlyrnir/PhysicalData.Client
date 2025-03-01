using PhysicalData.Presentation.Filter;
using PhysicalData.Presentation.Model;
using PhysicalData.Presentation.Result;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Interface
{
    public interface ITimePeriodService
    {
        ValueTask<ApiResult<PagedResult<TimePeriod>>> FindTimePeriodByFilter(TimePeriodFilter fltTimePeriod, CancellationToken tknCancellation);
        ValueTask<ApiResult<TimePeriod>> FindTimePeriodById(Guid guTimePeriodId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryCreateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryDeleteTimePeriod(Guid guTimePeriodId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryUpdateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation);
    }
}