using Blazored.LocalStorage;
using LocalizationComponent.Interface;
using MessageQueueComponent;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PassportCheckpoint.Interface;
using Presentation;
using Presentation.Authentication;
using Presentation.Interface;
using Presentation.Localization;
using Presentation.Model;
using Presentation.Service;
using Presentation.Storage;
using Presentation.Token;

var webBuilder = WebAssemblyHostBuilder.CreateDefault(args);
webBuilder.RootComponents.Add<App>("#app");
webBuilder.RootComponents.Add<HeadOutlet>("head::after");

webBuilder.Services.AddSingleton(TimeProvider.System);

webBuilder.Services.AddScoped(srvProvider => new HttpClient
{
    BaseAddress = new Uri(webBuilder.HostEnvironment.BaseAddress)
});

webBuilder.Services.AddTransient<AuthenticationHeaderHandler>();
//webBuilder.Services.AddTransient<HttpRequestHandler>();

//webBuilder.Services.AddLogging(logBuilder =>
//{
//    //builder.ClearProviders();
//    //builder.AddProvider();
//    logBuilder.SetMinimumLevel(LogLevel.Warning);
//});

webBuilder.Services.AddHttpClient(HttpClientName.Client.Root, httpClient =>
{
    httpClient.BaseAddress = new Uri(webBuilder.HostEnvironment.BaseAddress);
});

webBuilder.Services.AddHttpClient(HttpClientName.Api.Health, httpClient =>
{
    httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
});

webBuilder.Services.AddHttpClient(HttpClientName.Api.Anonymous, httpClient =>
{
    httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
});
    //.AddHttpMessageHandler<HttpRequestHandler>();

webBuilder.Services.AddHttpClient(HttpClientName.Api.WithAuthentication, httpClient =>
{
    httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
})
    .AddHttpMessageHandler<AuthenticationHeaderHandler>();
//.AddHttpMessageHandler<HttpRequestHandler>();

//webBuilder.Services.AddScoped<IApplicationService, ApplicationService>();

webBuilder.Services.AddTransient<IPassportCredential, PassportCredential>();

webBuilder.Services.AddBlazoredLocalStorage();
webBuilder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
webBuilder.Services.AddScoped<IPreferenceStorageService, PreferenceStorageService>();

webBuilder.Services.AddScoped<IPassportStateProvider, PassportStateProvider>();
webBuilder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

webBuilder.Services.AddScoped<ILocalizationStateProvider, LocalizationStateProvider>();

webBuilder.Services.AddMessageQueue();

webBuilder.Services.AddScoped<IPhysicalDimensionService, PhysicalDimensionService>();
webBuilder.Services.AddScoped<ITimePeriodService, TimePeriodService>();

await webBuilder.Build().RunAsync();
