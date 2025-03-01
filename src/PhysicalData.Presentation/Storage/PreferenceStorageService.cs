using Blazored.LocalStorage;
using Microsoft.Extensions.Configuration;
using PhysicalData.Presentation.Interface;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Storage
{
    public class PreferenceStorageService : IPreferenceStorageService
    {
        private readonly IConfiguration cfgConfiguration;
        private readonly ILocalStorageService lssLocalStorage;

        public PreferenceStorageService(IConfiguration cfgConfiguration, ILocalStorageService lssLocalStorage)
        {
            this.cfgConfiguration = cfgConfiguration;
            this.lssLocalStorage = lssLocalStorage;
        }

        public async ValueTask<string?> ReadPreferredCultureNameAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return null;

            return await lssLocalStorage.GetItemAsync<string>(cfgConfiguration["Culture:NameStorageKey"]!, tknCancellation);
        }

        public async ValueTask<string?> ReadPreferredThemeNameAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return null;

            return await lssLocalStorage.GetItemAsync<string>(cfgConfiguration["Theme:NameStorageKey"]!, tknCancellation);
        }

        public async ValueTask<bool> WriteCultureNameAsync(string? sCultureName, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            if (string.IsNullOrWhiteSpace(sCultureName) == true)
                return false;

            await lssLocalStorage.SetItemAsync(cfgConfiguration["Culture:NameStorageKey"]!, sCultureName, tknCancellation);

            return true;
        }

        public async ValueTask<bool> WriteThemeNameAsync(string? sThemeName, CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            if (string.IsNullOrWhiteSpace(sThemeName) == true)
                return false;

            await lssLocalStorage.SetItemAsync(cfgConfiguration["Theme:NameStorageKey"]!, sThemeName, tknCancellation);

            return true;
        }

        public async ValueTask<bool> ResetSettingAsync(CancellationToken tknCancellation)
        {
            if (tknCancellation.IsCancellationRequested)
                return false;

            await lssLocalStorage.RemoveItemAsync(cfgConfiguration["Culture:NameStorageKey"]!, tknCancellation);
            await lssLocalStorage.RemoveItemAsync(cfgConfiguration["Theme:NameStorageKey"]!, tknCancellation);

            return true;
        }
    }
}
