using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Passport.Contract.v01.Request.Authentication;
using Passport.Contract.v01.Response.Authentication;
using Passport.Contract.v01.Response.Passport;
using Passport.Contract.v01.Response.PassportVisa;
using PassportCheckpoint.Interface;
using PhysicalData.Presentation.Extension;
using PhysicalData.Presentation.Interface;
using PhysicalData.Presentation.Result;
using Presentation.Result;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Authentication
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IConfiguration cfgConfiguration;
        private readonly IHttpClientFactory httpClientFactory;

        private readonly TimeProvider prvTime;
        private readonly ITokenStorageService tknStorage;

        private readonly JsonSerializerOptions jsonOption;

        private const string JwtClaimId = "PASSPORT_ID";

        private readonly ILogger<AuthenticationService> logAuthenticationService;

        public AuthenticationService(ITokenStorageService tknStorage, TimeProvider prvTime, IHttpClientFactory httpClientFactory, IConfiguration cfgConfiguration, ILogger<AuthenticationService> logAuthenticationService)
        {
            this.cfgConfiguration = cfgConfiguration;
            this.httpClientFactory = httpClientFactory;

            this.prvTime = prvTime;
            this.tknStorage = tknStorage;

            jsonOption = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            this.logAuthenticationService = logAuthenticationService;
        }

        /// <inheritdoc/>
        public async ValueTask<ApiResult<bool>> InitializeBearerTokenAsync(IPassportCredential bwpCredential, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested == true)
                return new ApiResult<bool>(DefaultApiError.TaskAborted);

            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName.Api.Anonymous);

            GenerateAuthenticationTokenByCredentialRequest rqstToken = new GenerateAuthenticationTokenByCredentialRequest()
            {
                Credential = bwpCredential.Credential,
                Provider = cfgConfiguration["Authentication:Provider"]!,
                Signature = bwpCredential.Signature
            };

            HttpResponseMessage httpMessageGenerateBearerTokenByCredential = await httpClient.PostAsJsonAsync(EndpointRoute.Authentication.Token, rqstToken, tknCancellation);

            if (httpMessageGenerateBearerTokenByCredential.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<bool>(new ApiError() { Code = httpMessageGenerateBearerTokenByCredential.StatusCode.ToString(), Description = await httpMessageGenerateBearerTokenByCredential.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessageGenerateBearerTokenByCredential.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessageGenerateBearerTokenByCredential.StatusCode != HttpStatusCode.OK)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            AuthenticationTokenResponse? rspnAuthenticationToken = await httpMessageGenerateBearerTokenByCredential.Content.ReadFromJsonAsync<AuthenticationTokenResponse>(jsonOption, tknCancellation);

            if (rspnAuthenticationToken is null)
                return new ApiResult<bool>(DefaultApiError.DeserializationReturnsNull);

            bool bResult = false;

            bResult = await tknStorage.WriteAuthenticationTokenAsync(rspnAuthenticationToken.Token, tknCancellation);
            bResult = await tknStorage.WriteProviderAsync(rspnAuthenticationToken.Provider, tknCancellation);
            bResult = await tknStorage.WriteRefreshTokenAsync(rspnAuthenticationToken.RefreshToken, tknCancellation);

            return new ApiResult<bool>(bResult);
        }

        /// <inheritdoc/>
        public async ValueTask<ApiResult<bool>> RefreshBearerTokenAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested == true)
                return new ApiResult<bool>(DefaultApiError.TaskAborted);

            string? sToken = await tknStorage.ReadAuthenticationTokenAsync(tknCancellation);
            string? sProvider = await tknStorage.ReadProviderAsync(tknCancellation);
            string? sRefreshToken = await tknStorage.ReadRefreshTokenAsync(tknCancellation);

            JsonWebTokenHandler jwtHandler = new JsonWebTokenHandler();

            if (jwtHandler.CanReadToken(sToken) == false)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.ReturnsInvalidToken);

            JsonWebToken jwtToken = jwtHandler.ReadJsonWebToken(sToken);

            Guid guPassportId = Guid.Empty;

            foreach (Claim clmActual in jwtToken.Claims)
            {
                switch (clmActual.Type)
                {
                    case JwtClaimId:
                        guPassportId = Guid.Parse(clmActual.Value);
                        break;
                    default:
                        continue;
                }
            }

            if (sProvider is null)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.ReturnsInvalidValue);

            if (sProvider != cfgConfiguration["Authentication:Provider"]!)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.ReturnsInvalidValue);

            if (sRefreshToken is null)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.ReturnsInvalidValue);

            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName.Api.WithAuthentication);

            GenerateAuthenticationTokenByRefreshTokenRequest rqstToken = new GenerateAuthenticationTokenByRefreshTokenRequest()
            {
                PassportId = guPassportId,
                Provider = cfgConfiguration["Authentication:Provider"]!,
                RefreshToken = sRefreshToken
            };

            HttpResponseMessage httpMessageGenerateBearerTokenByRefreshToken = await httpClient.PostAsJsonAsync(EndpointRoute.Authentication.RefreshToken, rqstToken, tknCancellation);

            if (httpMessageGenerateBearerTokenByRefreshToken.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<bool>(new ApiError() { Code = httpMessageGenerateBearerTokenByRefreshToken.StatusCode.ToString(), Description = await httpMessageGenerateBearerTokenByRefreshToken.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessageGenerateBearerTokenByRefreshToken.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessageGenerateBearerTokenByRefreshToken.StatusCode != HttpStatusCode.OK)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            AuthenticationTokenResponse? rspnAuthenticationToken = await httpMessageGenerateBearerTokenByRefreshToken.Content.ReadFromJsonAsync<AuthenticationTokenResponse>(jsonOption, tknCancellation);

            if (rspnAuthenticationToken is null)
                return new ApiResult<bool>(DefaultApiError.DeserializationReturnsNull);

            bool bResult = false;

            bResult = await tknStorage.WriteAuthenticationTokenAsync(rspnAuthenticationToken.Token, tknCancellation);
            bResult = await tknStorage.WriteProviderAsync(rspnAuthenticationToken.Provider, tknCancellation);
            bResult = await tknStorage.WriteRefreshTokenAsync(rspnAuthenticationToken.RefreshToken, tknCancellation);

            if (bResult == false)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.TokenIsNotStored);

            return new ApiResult<bool>(bResult);
        }

        /// <inheritdoc/>
        public async ValueTask<ApiResult<IPassport>> FindPassport(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested == true)
                return new ApiResult<IPassport>(DefaultApiError.TaskAborted);

            string? sToken = await tknStorage.ReadAuthenticationTokenAsync(tknCancellation);

            if (TryParsePassportId(sToken, out Guid guPassportId) == false)
                return new ApiResult<IPassport>(DefaultApiError.TokenStorage.ReturnsInvalidToken);

            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName.Api.WithAuthentication);

            ApiResult<IEnumerable<IPassportVisa>> enumPassportVisa = await GetPassportVisa(guPassportId, httpClient, tknCancellation);

            return await enumPassportVisa.MatchAsync(
                msgError =>
                {
                    return new ApiResult<IPassport>(new ApiError() { Code = msgError.Code, Description = msgError.Description });
                },
                async enumPassportVisa =>
                {
                    return await GetPassportById(guPassportId, enumPassportVisa, httpClient, tknCancellation);
                });
        }

        private async ValueTask<ApiResult<IEnumerable<IPassportVisa>>> GetPassportVisa(Guid guPassportId, HttpClient httpClient, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested == true)
                return new ApiResult<IEnumerable<IPassportVisa>>(DefaultApiError.TaskAborted);

            Uri uriFindPassportVisaByPassportId = new Uri(httpClient.BaseAddress!, EndpointRoute.PassportVisa.GetByPassportId)
                .AddQueryParameter("guPassportIdToFind", guPassportId);

            HttpResponseMessage httpMessageFindPassportVisaByPassportId = await httpClient.GetAsync(uriFindPassportVisaByPassportId, tknCancellation);

            if (httpMessageFindPassportVisaByPassportId.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<IEnumerable<IPassportVisa>>(new ApiError() { Code = httpMessageFindPassportVisaByPassportId.StatusCode.ToString(), Description = await httpMessageFindPassportVisaByPassportId.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessageFindPassportVisaByPassportId.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<IEnumerable<IPassportVisa>>(DefaultApiError.Api.NotAuthorized);

            if (httpMessageFindPassportVisaByPassportId.StatusCode != HttpStatusCode.OK)
                return new ApiResult<IEnumerable<IPassportVisa>>(DefaultApiError.UnexpectedStatusCode);

            IEnumerable<PassportVisaResponse>? rspnPassportVisa = await httpMessageFindPassportVisaByPassportId.Content.ReadFromJsonAsync<IEnumerable<PassportVisaResponse>>(jsonOption, tknCancellation);

            if (rspnPassportVisa is null)
                return new ApiResult<IEnumerable<IPassportVisa>>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<IEnumerable<IPassportVisa>>(rspnPassportVisa.ParseToModel());
        }

        private async ValueTask<ApiResult<IPassport>> GetPassportById(Guid guPassportId, IEnumerable<IPassportVisa> enumPassportVisa, HttpClient httpClient, CancellationToken tknCancellation)
        {
            HttpResponseMessage httpMessageFindPassportById = await httpClient.GetAsync(EndpointRoute.Passport.GetById(guPassportId), tknCancellation);

            if (httpMessageFindPassportById.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<IPassport>(new ApiError() { Code = httpMessageFindPassportById.StatusCode.ToString(), Description = await httpMessageFindPassportById.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessageFindPassportById.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<IPassport>(DefaultApiError.Api.NotAuthorized);

            if (httpMessageFindPassportById.StatusCode != HttpStatusCode.OK)
                return new ApiResult<IPassport>(DefaultApiError.UnexpectedStatusCode);

            PassportResponse? rspnPassport = await httpMessageFindPassportById.Content.ReadFromJsonAsync<PassportResponse>(jsonOption, tknCancellation);

            if (rspnPassport is null)
                return new ApiResult<IPassport>(DefaultApiError.DeserializationReturnsNull);

            return new ApiResult<IPassport>(rspnPassport.ParseToModel(enumPassportVisa));
        }

        /// <inheritdoc/>
        public async ValueTask<ApiResult<string>> ReadJwtTokenAsync(CancellationToken tknCancellation)
        {
            string? sToken = await tknStorage.ReadAuthenticationTokenAsync(tknCancellation);

            if (string.IsNullOrWhiteSpace(sToken) == true)
            {
                await ResetJwtTokenAsync(tknCancellation);

                return new ApiResult<string>(DefaultApiError.TokenStorage.ReturnsInvalidToken);
            }

            return new ApiResult<string>(sToken);
        }

        /// <inheritdoc/>
        public async ValueTask<ApiResult<bool>> ResetJwtTokenAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested == true)
                return new ApiResult<bool>(DefaultApiError.TaskAborted);

            string? sToken = await tknStorage.ReadAuthenticationTokenAsync(tknCancellation);

            if (TryParsePassportId(sToken, out Guid guPassportId) == false)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.ReturnsInvalidToken);

            HttpClient httpClient = httpClientFactory.CreateClient(HttpClientName.Api.WithAuthentication);

            ResetRefreshTokenByPassportIdRequest rqstToken = new ResetRefreshTokenByPassportIdRequest()
            {
                Provider = cfgConfiguration["Authentication:Provider"]!,
                PassportId = guPassportId
            };

            HttpResponseMessage httpMessageGenerateBearerTokenByCredential = await httpClient.PostAsJsonAsync(EndpointRoute.Authentication.Reset, rqstToken, tknCancellation);

            if (httpMessageGenerateBearerTokenByCredential.StatusCode == HttpStatusCode.BadRequest)
                return new ApiResult<bool>(new ApiError() { Code = httpMessageGenerateBearerTokenByCredential.StatusCode.ToString(), Description = await httpMessageGenerateBearerTokenByCredential.Content.ReadAsStringAsync(tknCancellation) });

            if (httpMessageGenerateBearerTokenByCredential.StatusCode == HttpStatusCode.Unauthorized)
                return new ApiResult<bool>(DefaultApiError.Api.NotAuthorized);

            if (httpMessageGenerateBearerTokenByCredential.StatusCode != HttpStatusCode.OK)
                return new ApiResult<bool>(DefaultApiError.UnexpectedStatusCode);

            bool bResult = await tknStorage.ResetTokenAsync(tknCancellation);

            if (bResult == false)
                return new ApiResult<bool>(DefaultApiError.TokenStorage.TokenIsNotRemoved);

            return new ApiResult<bool>(bResult);
        }

        private bool TryParsePassportId(string? sToken, out Guid guPassportId)
        {
            bool bResult = false;
            guPassportId = Guid.Empty;

            if (sToken is null)
                return bResult;

            JsonWebTokenHandler jwtHandler = new JsonWebTokenHandler();

            if (jwtHandler.CanReadToken(sToken) == false)
                return bResult;

            JsonWebToken jwtToken = jwtHandler.ReadJsonWebToken(sToken);

            foreach (Claim clmActual in jwtToken.Claims)
            {
                switch (clmActual.Type)
                {
                    case JwtClaimId:
                        guPassportId = Guid.Parse(clmActual.Value);
                        bResult = true;
                        break;
                    default:
                        continue;
                }
            }

            return bResult;
        }
    }

    public class JwtDecoder
    {
        public static T? DecodeToken<T>(string token)
        {
            var parts = token.Split('.');
            if (parts.Length != 3)
            {
                throw new ArgumentException("Invalid JWT token.");
            }

            var payload = parts[1];
            var jsonBytes = Convert.FromBase64String(PadBase64(payload));
            return JsonSerializer.Deserialize<T>(jsonBytes);
        }

        private static string PadBase64(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: return base64 + "==";
                case 3: return base64 + "=";
                default: return base64;
            }
        }
    }
}
