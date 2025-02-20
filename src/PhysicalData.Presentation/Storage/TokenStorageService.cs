using Blazored.LocalStorage;
using PhysicalData.Presentation.Interface;

namespace PhysicalData.Presentation.Storage
{
    public class TokenStorageService : ITokenStorageService
    {
        private readonly IConfiguration cfgConfiguration;
        private readonly ILocalStorageService lssLocalStorage;

        public TokenStorageService(IConfiguration cfgConfiguration, ILocalStorageService lssLocalStorage)
        {
            this.cfgConfiguration = cfgConfiguration;
            this.lssLocalStorage = lssLocalStorage;
        }

        public async ValueTask<string?> ReadAuthenticationTokenAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return null;

            return await lssLocalStorage.GetItemAsync<string>(cfgConfiguration["Authentication:AuthenticationTokenStorageKey"]!, tknCancellation);
        }



        public async ValueTask<string?> ReadProviderAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return null;

            return await lssLocalStorage.GetItemAsync<string>(cfgConfiguration["Authentication:ProviderStorageKey"]!, tknCancellation);
        }

        public async ValueTask<string?> ReadRefreshTokenAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return null;

            return await lssLocalStorage.GetItemAsync<string>(cfgConfiguration["Authentication:RefreshTokenStorageKey"]!, tknCancellation);
        }

        public async ValueTask<bool> WriteAuthenticationTokenAsync(string? sToken, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            if (string.IsNullOrWhiteSpace(sToken) == true)
                return false;

            await lssLocalStorage.SetItemAsync(cfgConfiguration["Authentication:AuthenticationTokenStorageKey"]!, sToken, tknCancellation);

            return true;
        }

        public async ValueTask<bool> WriteProviderAsync(string? sToken, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            if (string.IsNullOrWhiteSpace(sToken) == true)
                return false;

            await lssLocalStorage.SetItemAsync(cfgConfiguration["Authentication:ProviderStorageKey"]!, sToken, tknCancellation);

            return true;
        }

        public async ValueTask<bool> WriteRefreshTokenAsync(string? sToken, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            if (string.IsNullOrWhiteSpace(sToken) == true)
                return false;

            await lssLocalStorage.SetItemAsync(cfgConfiguration["Authentication:RefreshTokenStorageKey"]!, sToken, tknCancellation);

            return true;
        }

        public async ValueTask<bool> ResetTokenAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            await lssLocalStorage.RemoveItemAsync(cfgConfiguration["Authentication:AuthenticationTokenStorageKey"]!, tknCancellation);
            await lssLocalStorage.RemoveItemAsync(cfgConfiguration["Authentication:ProviderStorageKey"]!, tknCancellation);
            await lssLocalStorage.RemoveItemAsync(cfgConfiguration["Authentication:RefreshTokenStorageKey"]!, tknCancellation);

            return true;
        }
    }
}
