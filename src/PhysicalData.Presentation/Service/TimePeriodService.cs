using PhysicalData.Contract.v01.Request.TimePeriod;
using PhysicalData.Contract.v01.Response.TimePeriod;
using PhysicalData.Presentation.Extension;
using PhysicalData.Presentation.Filter;
using PhysicalData.Presentation.Interface;
using PhysicalData.Presentation.Model;
using PhysicalData.Presentation.Result;
using Presentation.Result;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace PhysicalData.Presentation.Service
{
    public class TimePeriodService : ITimePeriodService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOption;

        public TimePeriodService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(HttpClientName.Api.WithAuthentication);
            jsonOption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async ValueTask<ApiResult<PagedResult<TimePeriod>>> FindTimePeriodByFilter(TimePeriodFilter fltTimePeriod, CancellationToken tknCancellation)
        {
            FindTimePeriodByFilterRequest rqstTimePeriod = new FindTimePeriodByFilterRequest()
            {
                Magnitude = fltTimePeriod.Magnitude,
                Offset = fltTimePeriod.Offset,
                PhysicalDimensionId = fltTimePeriod.PhysicalDimensionId,
                Page = fltTimePeriod.Page,
                PageSize = fltTimePeriod.PageSize
            };

            Uri uriEndpoint = new Uri(httpClient.BaseAddress!, EndpointRoute.TimePeriod.GetUnspecific)
                .AddQueryParameter("Magnitude", rqstTimePeriod.Magnitude)
                .AddQueryParameter("Offset", rqstTimePeriod.Offset)
                .AddQueryParameter("PhysicalDimensionId", rqstTimePeriod.PhysicalDimensionId)
                .AddQueryParameter("Page", rqstTimePeriod.Page)
                .AddQueryParameter("PageSize", rqstTimePeriod.PageSize);

            HttpResponseMessage httpMessage = await httpClient.GetAsync(uriEndpoint, tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<PagedResult<TimePeriod>>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<PagedResult<TimePeriod>>(DefaultApiError.Api.NotAuthenticated);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<PagedResult<TimePeriod>>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<PagedResult<TimePeriod>>(DefaultApiError.UnexpectedStatusCode);

            TimePeriodByFilterResponse? rspnTimePeriod = await httpMessage.Content.ReadFromJsonAsync<TimePeriodByFilterResponse>(jsonOption, tknCancellation);

            if (rspnTimePeriod is null)
                return new ApiResult<PagedResult<TimePeriod>>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<PagedResult<TimePeriod>>(new PagedResult<TimePeriod>()
            {
                Content = rspnTimePeriod.TimePeriod.ParseToModel(),
                ResultCount = rspnTimePeriod.ResultCount
            });
        }

        public async ValueTask<ApiResult<TimePeriod>> FindTimePeriodById(Guid guTimePeriodId, CancellationToken tknCancellation)
        {
            HttpResponseMessage httpMessage = await httpClient.GetAsync(EndpointRoute.TimePeriod.GetById(guTimePeriodId), tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<TimePeriod>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<TimePeriod>(DefaultApiError.Api.NotAuthenticated);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<TimePeriod>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<TimePeriod>(DefaultApiError.UnexpectedStatusCode);

            TimePeriodByIdResponse? rspnTimePeriod = await httpMessage.Content.ReadFromJsonAsync<TimePeriodByIdResponse>(jsonOption, tknCancellation);

            if (rspnTimePeriod is null)
                return new ApiResult<TimePeriod>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<TimePeriod>(rspnTimePeriod.ParseToModel());
        }

        public async ValueTask<ApiResult<bool>> TryCreateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation)
        {
            CreateTimePeriodRequest rqstTimePeriod = new CreateTimePeriodRequest()
            {
                Magnitude = pdTimePeriod.Magnitude,
                Offset = pdTimePeriod.Offset,
                PhysicalDimensionId = pdTimePeriod.PhysicalDimensionId
            };

            HttpResponseMessage httpMessage = await httpClient.PostAsJsonAsync(EndpointRoute.TimePeriod.Create, rqstTimePeriod, tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<bool>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthenticated);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.Created)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            return new ApiResult<bool>(true);
        }

        public async ValueTask<ApiResult<bool>> TryDeleteTimePeriod(Guid guTimePeriodId, CancellationToken tknCancellation)
        {
            Uri uriEndpoint = new Uri(httpClient.BaseAddress!, EndpointRoute.TimePeriod.Delete)
                .AddQueryParameter("guTimePeriodId", guTimePeriodId);

            HttpResponseMessage httpMessage = await httpClient.DeleteAsync(uriEndpoint, tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<bool>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            return new ApiResult<bool>(true);
        }

        public async ValueTask<ApiResult<bool>> TryUpdateTimePeriod(TimePeriod pdTimePeriod, CancellationToken tknCancellation)
        {
            UpdateTimePeriodRequest rqstPhysicalDimension = new UpdateTimePeriodRequest()
            {
                TimePeriodId = pdTimePeriod.Id,
                ConcurrencyStamp = pdTimePeriod.ConcurrencyStamp,
                Magnitude = pdTimePeriod.Magnitude,
                Offset = pdTimePeriod.Offset,
                PhysicalDimensionId = pdTimePeriod.PhysicalDimensionId
            };

            HttpResponseMessage httpMessage = await httpClient.PutAsJsonAsync(EndpointRoute.TimePeriod.Update, rqstPhysicalDimension, tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest || httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            return new ApiResult<bool>(true);
        }
    }
}