using PhysicalData.Contract.v01.Request.PhysicalDimension;
using PhysicalData.Contract.v01.Response.PhysicalDimension;
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
    public class PhysicalDimensionService : IPhysicalDimensionService
    {
        private readonly HttpClient httpClient;
        private readonly JsonSerializerOptions jsonOption;

        public PhysicalDimensionService(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient(HttpClientName.Api.WithAuthentication);
            jsonOption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async ValueTask<ApiResult<PagedResult<PhysicalDimension>>> FindPhysicalDimensionByFilter(PhysicalDimensionFilter fltPhysicalDimension, CancellationToken tknCancellation)
        {
            FindPhysicalDimensionByFilterRequest rqstPhysicalDimension = new FindPhysicalDimensionByFilterRequest()
            {
                ConversionFactorToSI = fltPhysicalDimension.ConversionFactorToSI,
                CultureName = fltPhysicalDimension.CultureName,
                ExponentOfAmpere = fltPhysicalDimension.ExponentOfAmpere,
                ExponentOfCandela = fltPhysicalDimension.ExponentOfCandela,
                ExponentOfKelvin = fltPhysicalDimension.ExponentOfKelvin,
                ExponentOfKilogram = fltPhysicalDimension.ExponentOfKilogram,
                ExponentOfMetre = fltPhysicalDimension.ExponentOfMetre,
                ExponentOfMole = fltPhysicalDimension.ExponentOfMole,
                ExponentOfSecond = fltPhysicalDimension.ExponentOfSecond,
                Name = fltPhysicalDimension.Name,
                Symbol = fltPhysicalDimension.Symbol,
                Unit = fltPhysicalDimension.Unit,
                Page = fltPhysicalDimension.Page,
                PageSize = fltPhysicalDimension.PageSize
            };

            Uri uriEndpoint = new Uri(httpClient.BaseAddress!, EndpointRoute.PhysicalDimension.GetUnspecific)
                .AddQueryParameter("ConversionFactorToSI", rqstPhysicalDimension.ConversionFactorToSI)
                .AddQueryParameter("CultureName", rqstPhysicalDimension.CultureName)
                .AddQueryParameter("ExponentOfAmpere", rqstPhysicalDimension.ExponentOfAmpere)
                .AddQueryParameter("ExponentOfCandela", rqstPhysicalDimension.ExponentOfCandela)
                .AddQueryParameter("ExponentOfKelvin", rqstPhysicalDimension.ExponentOfKelvin)
                .AddQueryParameter("ExponentOfKilogram", rqstPhysicalDimension.ExponentOfKilogram)
                .AddQueryParameter("ExponentOfMetre", rqstPhysicalDimension.ExponentOfMetre)
                .AddQueryParameter("ExponentOfMole", rqstPhysicalDimension.ExponentOfMole)
                .AddQueryParameter("ExponentOfSecond", rqstPhysicalDimension.ExponentOfSecond)
                .AddQueryParameter("Name", rqstPhysicalDimension.Name)
                .AddQueryParameter("Symbol", rqstPhysicalDimension.Symbol)
                .AddQueryParameter("Unit", rqstPhysicalDimension.Unit)
                .AddQueryParameter("Page", rqstPhysicalDimension.Page)
                .AddQueryParameter("PageSize", rqstPhysicalDimension.PageSize);

            HttpResponseMessage httpMessage = await httpClient.GetAsync(uriEndpoint, tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<PagedResult<PhysicalDimension>>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<PagedResult<PhysicalDimension>>(DefaultApiError.Api.NotAuthenticated);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<PagedResult<PhysicalDimension>>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<PagedResult<PhysicalDimension>>(DefaultApiError.UnexpectedStatusCode);

            PhysicalDimensionByFilterResponse? rspnPhysicalDimension = await httpMessage.Content.ReadFromJsonAsync<PhysicalDimensionByFilterResponse>(jsonOption, tknCancellation);

            if (rspnPhysicalDimension is null)
                return new ApiResult<PagedResult<PhysicalDimension>>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<PagedResult<PhysicalDimension>>(new PagedResult<PhysicalDimension>()
            {
                Content = rspnPhysicalDimension.PhysicalDimension.ParseToModel(),
                ResultCount = rspnPhysicalDimension.ResultCount
            });
        }

        public async ValueTask<ApiResult<PhysicalDimension>> FindPhysicalDimensionById(Guid guPhysicalDimensionId, CancellationToken tknCancellation)
        {
            HttpResponseMessage httpMessage = await httpClient.GetAsync(EndpointRoute.PhysicalDimension.GetById(guPhysicalDimensionId), tknCancellation);

            if (httpMessage.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<PhysicalDimension>(new ApiError() { Code = httpMessage.StatusCode.ToString(), Description = await httpMessage.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessage.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<PhysicalDimension>(DefaultApiError.Api.NotAuthenticated);

            if (httpMessage.StatusCode == HttpStatusCode.Forbidden)
                return new ApiResult<PhysicalDimension>(DefaultApiError.Api.NotAuthorized);

            if (httpMessage.StatusCode != HttpStatusCode.OK)
                return new ApiResult<PhysicalDimension>(DefaultApiError.UnexpectedStatusCode);

            PhysicalDimensionByIdResponse? rspnPhysicalDimension = await httpMessage.Content.ReadFromJsonAsync<PhysicalDimensionByIdResponse>(jsonOption, tknCancellation);

            if (rspnPhysicalDimension is null)
                return new ApiResult<PhysicalDimension>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<PhysicalDimension>(rspnPhysicalDimension.ParseToModel());
        }

        public async ValueTask<ApiResult<bool>> TryCreatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation)
        {
            CreatePhysicalDimensionRequest rqstPhysicalDimension = new CreatePhysicalDimensionRequest()
            {
                ConversionFactorToSI = pdPhysicalDimension.ConversionFactorToSI,
                CultureName = pdPhysicalDimension.CultureName,
                ExponentOfAmpere = pdPhysicalDimension.ExponentOfAmpere,
                ExponentOfCandela = pdPhysicalDimension.ExponentOfCandela,
                ExponentOfKelvin = pdPhysicalDimension.ExponentOfKelvin,
                ExponentOfKilogram = pdPhysicalDimension.ExponentOfKilogram,
                ExponentOfMetre = pdPhysicalDimension.ExponentOfMetre,
                ExponentOfMole = pdPhysicalDimension.ExponentOfMole,
                ExponentOfSecond = pdPhysicalDimension.ExponentOfSecond,
                Name = pdPhysicalDimension.Name,
                Symbol = pdPhysicalDimension.Symbol,
                Unit = pdPhysicalDimension.Unit
            };

            HttpResponseMessage httpMessage = await httpClient.PostAsJsonAsync(EndpointRoute.PhysicalDimension.Create, rqstPhysicalDimension, tknCancellation);

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

        public async ValueTask<ApiResult<bool>> TryDeletePhysicalDimension(Guid guPhysicalDimensionId, CancellationToken tknCancellation)
        {
            Uri uriEndpoint = new Uri(httpClient.BaseAddress!, EndpointRoute.PhysicalDimension.Delete)
                .AddQueryParameter("guPhysicalDimensionId", guPhysicalDimensionId);

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

        public async ValueTask<ApiResult<bool>> TryUpdatePhysicalDimension(PhysicalDimension pdPhysicalDimension, CancellationToken tknCancellation)
        {
            UpdatePhysicalDimensionRequest rqstPhysicalDimension = new UpdatePhysicalDimensionRequest()
            {
                PhysicalDimensionId = pdPhysicalDimension.Id,
                ConcurrencyStamp = pdPhysicalDimension.ConcurrencyStamp,
                ConversionFactorToSI = pdPhysicalDimension.ConversionFactorToSI,
                CultureName = pdPhysicalDimension.CultureName,
                ExponentOfAmpere = pdPhysicalDimension.ExponentOfAmpere,
                ExponentOfCandela = pdPhysicalDimension.ExponentOfCandela,
                ExponentOfKelvin = pdPhysicalDimension.ExponentOfKelvin,
                ExponentOfKilogram = pdPhysicalDimension.ExponentOfKilogram,
                ExponentOfMetre = pdPhysicalDimension.ExponentOfMetre,
                ExponentOfMole = pdPhysicalDimension.ExponentOfMole,
                ExponentOfSecond = pdPhysicalDimension.ExponentOfSecond,
                Name = pdPhysicalDimension.Name,
                Symbol = pdPhysicalDimension.Symbol,
                Unit = pdPhysicalDimension.Unit
            };

            HttpResponseMessage httpMessage = await httpClient.PutAsJsonAsync(EndpointRoute.PhysicalDimension.Update, rqstPhysicalDimension, tknCancellation);

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