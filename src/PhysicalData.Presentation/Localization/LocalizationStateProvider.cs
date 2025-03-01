using LocalizationComponent;
using LocalizationComponent.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace PhysicalData.Presentation.Localization
{
    public sealed class LocalizationStateProvider : LocalizationStateEventImpl, ILocalizationStateProvider
    {
        private readonly HttpClient httpClient;
        private JsonSerializerOptions jsonSerializerOption;

        private readonly ILogger<LocalizationStateProvider> logLocalization;

        public LocalizationStateProvider(IHttpClientFactory httpFactory, ILogger<LocalizationStateProvider> logLocalization)
        {
            httpClient = httpFactory.CreateClient(HttpClientName.Client.Root);

            jsonSerializerOption = new JsonSerializerOptions()
            {
                Converters =
                {
                    new DictionaryConverter(CultureName.en_GB)
                }
            };

            this.logLocalization = logLocalization;
        }

        public async Task ChangeCultureNameAsync(string sCultureName, CancellationToken tknCancellation)
        {
            jsonSerializerOption = new JsonSerializerOptions()
            {
                Converters =
                {
                    new DictionaryConverter(sCultureName)
                }
            };

            LocalizationState lclState = await GetLocalizationStateAsync(sCultureName, tknCancellation);

            NotifyLocalizationStateChanged(Task.FromResult(lclState));
        }

        private async Task<LocalizationState> GetLocalizationStateAsync(string sCultureName, CancellationToken tknCancellation)
        {
            HttpResponseMessage rspnMessage = await httpClient.GetAsync("resource/Dictionary.json", CancellationToken.None);

            if (rspnMessage.IsSuccessStatusCode == false)
                return LocalizationState.Initialize(CultureName.en_GB, new Dictionary<string, string>());

            IDictionary<string, string>? dictDictionary = await rspnMessage.Content.ReadFromJsonAsync<Dictionary<string, string>>(jsonSerializerOption, CancellationToken.None);

            if (dictDictionary is null)
                return LocalizationState.Initialize(CultureName.en_GB, new Dictionary<string, string>());

            return LocalizationState.Initialize(sCultureName, dictDictionary);
        }
    }
}
