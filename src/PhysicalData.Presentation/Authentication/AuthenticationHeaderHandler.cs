using Microsoft.Extensions.Logging;
using PassportCheckpoint.Interface;
using PhysicalData.Presentation.Interface;
using PhysicalData.Presentation.Result;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Authentication
{
    public class AuthenticationHeaderHandler : DelegatingHandler, IDisposable
    {
        private readonly IAuthenticationService srvAuthentication;

        private readonly ILogger<AuthenticationHeaderHandler> logAuthenticationHeaderHandler;

        private readonly IPassportStateProvider prvPassportState;

        public AuthenticationHeaderHandler(IPassportStateProvider prvPassportState, IAuthenticationService srvAuthentication, ILogger<AuthenticationHeaderHandler> logAuthenticationHeaderHandler)
        {
            this.srvAuthentication = srvAuthentication;

            this.logAuthenticationHeaderHandler = logAuthenticationHeaderHandler;

            this.prvPassportState = prvPassportState;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage httpRequest, CancellationToken tknCancellation)
        {
            ApiResult<string> rsltJwtToken = await srvAuthentication.ReadJwtTokenAsync(tknCancellation);

            HttpResponseMessage httpResponse = await rsltJwtToken.MatchAsync(
                msgError =>
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                },
                async sToken =>
                {
                    httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sToken);

                    return await base.SendAsync(httpRequest, tknCancellation);
                });

            if (httpResponse.StatusCode != System.Net.HttpStatusCode.Unauthorized)
                return httpResponse;

            ApiResult<bool> rsltRefreshToken = await srvAuthentication.RefreshBearerTokenAsync(tknCancellation);

            httpResponse = await rsltRefreshToken.MatchAsync(
                msgError =>
                {
                    return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                },
                async bResult =>
                {
                    ApiResult<string> rsltBearerToken = await srvAuthentication.ReadJwtTokenAsync(tknCancellation);

                    return await rsltBearerToken.MatchAsync(
                        msgError =>
                        {
                            return new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                        },
                        async sToken =>
                        {
                            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", sToken);

                            return await base.SendAsync(httpRequest, tknCancellation);
                        });
                });

            if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                await srvAuthentication.ResetJwtTokenAsync(tknCancellation);

            return httpResponse;
        }
    }
}
