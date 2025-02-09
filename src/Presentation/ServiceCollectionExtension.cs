//namespace Presentation
//{
//    public static class ServiceCollectionExtension
//    {
//        public static IServiceCollection AddPresentationService(this IServiceCollection cltService)
//        {
//            webBuilder.Services.AddScoped(srvProvider => new HttpClient
//            {
//                BaseAddress = new Uri(webBuilder.HostEnvironment.BaseAddress)
//            });

//            webBuilder.Services.AddTransient<AuthenticationHeaderHandler>();
//            webBuilder.Services.AddTransient<HttpRequestHandler>();

//            webBuilder.Services.AddLogging(logBuilder =>
//            {
//                //builder.ClearProviders();
//                //builder.AddProvider();
//                logBuilder.SetMinimumLevel(LogLevel.Warning);
//            });

//            webBuilder.Services.AddHttpClient(HttpClientName.Client.Root, httpClient =>
//            {
//                httpClient.BaseAddress = new Uri(webBuilder.HostEnvironment.BaseAddress);
//            });

//            webBuilder.Services.AddHttpClient(HttpClientName.Api.Health, httpClient =>
//            {
//                httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
//            });

//            webBuilder.Services.AddHttpClient(HttpClientName.Api.Anonymous, httpClient =>
//            {
//                httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
//            })
//                .AddHttpMessageHandler<HttpRequestHandler>();

//            webBuilder.Services.AddHttpClient(HttpClientName.Api.WithAuthentication, httpClient =>
//            {
//                httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
//            })
//                .AddHttpMessageHandler<AuthenticationHeaderHandler>()
//                .AddHttpMessageHandler<HttpRequestHandler>();

//            webBuilder.Services.AddScoped<IApplicationService, ApplicationService>();

//            webBuilder.Services.AddBlazoredLocalStorage();
//            webBuilder.Services.AddScoped<ITokenStorageService, TokenStorageService>();

//            webBuilder.Services.AddScoped<IPassportStateProvider, PassportStateProvider>();
//            webBuilder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//            webBuilder.Services.AddScoped<ILocalizationStateProvider, LocalizationStateProvider>();

//            webBuilder.Services.AddScoped<ITimeProvider, TimeProvider>();

//            return cltService;
//        }
//    }
//}
