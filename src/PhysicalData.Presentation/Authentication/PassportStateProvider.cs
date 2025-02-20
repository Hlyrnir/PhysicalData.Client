using PassportCheckpoint;
using PassportCheckpoint.Interface;
using PhysicalData.Presentation.Interface;
using PhysicalData.Presentation.Result;

namespace PhysicalData.Presentation.Authentication
{
    public sealed class PassportStateProvider : PassportStateEventImpl, IPassportStateProvider
    {
        private readonly IAuthenticationService srvAuthentication;

        private readonly TimeProvider prvTime;
        private readonly ILogger<PassportStateProvider> logPassportState;

        public PassportStateProvider(IAuthenticationService srvAuthentication, ILogger<PassportStateProvider> logPassportState, TimeProvider prvTime)
        {
            this.srvAuthentication = srvAuthentication;

            this.prvTime = prvTime;
            this.logPassportState = logPassportState;
        }

        public async Task RefreshAsync(CancellationToken tknCancellation)
        {
            PassportState ppState = await GetPassportStateAsync(tknCancellation);
            NotifyPassportStateChanged(Task.FromResult(ppState));
        }

        public async Task<bool> LogInAsync(IPassportCredential bwpCredential, CancellationToken tknCancellation)
        {
            ApiResult<bool> apiToken = await srvAuthentication.InitializeBearerTokenAsync(bwpCredential, tknCancellation);

            return await apiToken.MatchAsync(
                msgError =>
                {
                    logPassportState.LogWarning("'Log in' has failed. - {Message}", msgError.Description);
                    return false;
                },
                async bResult =>
                {
                    PassportState ppState = await GetPassportStateAsync(tknCancellation);
                    NotifyPassportStateChanged(Task.FromResult(ppState));

                    logPassportState.LogWarning("Log in at {DateTime} - {Credential}", prvTime.GetUtcNow(), bwpCredential.Credential);

                    return true;
                });
        }

        public async Task<bool> LogOutAsync(CancellationToken tknCancellation)
        {
            ApiResult<bool> apiResult = await srvAuthentication.ResetJwtTokenAsync(tknCancellation);

            return apiResult.Match(
                msgError =>
                {
                    logPassportState.LogWarning("'Log out' has failed. - {Message}", msgError.Description);

                    return false;
                },
                bResult =>
                {
                    PassportState ppState = PassportState.AsAnonymous();
                    NotifyPassportStateChanged(Task.FromResult(ppState));

                    return true;
                });
        }

        private async Task<PassportState> GetPassportStateAsync(CancellationToken tknCancellation)
        {
            ApiResult<IPassport> apiResult = await srvAuthentication.FindPassport(tknCancellation);

            return apiResult.Match(
                msgError => PassportState.AsAnonymous(),
                ppPassport => PassportState.Initialize(ppPassport));
        }
    }
}
