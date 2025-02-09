using Presentation.Filter;
using Presentation.Model;
using Presentation.Result;

namespace Presentation.Interface
{
    public interface ITimePeriodService
    {
        ValueTask<ApiResult<IEnumerable<TimePeriod>>> FindTimePeriodByFilter(TimePeriodFilter fltTimePeriod, CancellationToken tknCancellation);
        ValueTask<ApiResult<TimePeriod>> FindTimePeriodById(Guid guTimePeriodId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryCreateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryDeleteTimePeriod(Guid guTimePeriodId, CancellationToken tknCancellation);
        ValueTask<ApiResult<bool>> TryUpdateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation);
    }
}