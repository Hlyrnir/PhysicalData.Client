using Blazored.LocalStorage;
using LocalizationComponent.Interface;
using MessageQueueComponent;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PassportCheckpoint.Interface;
using PhysicalData.Presentation;
using PhysicalData.Presentation.Authentication;
using PhysicalData.Presentation.Interface;
using PhysicalData.Presentation.Localization;
using PhysicalData.Presentation.Model;
using PhysicalData.Presentation.Service;
using PhysicalData.Presentation.Storage;
using System;
using System.Net.Http;

var webBuilder = WebAssemblyHostBuilder.CreateDefault(args);
webBuilder.RootComponents.Add<App>("#app");
webBuilder.RootComponents.Add<HeadOutlet>("head::after");

webBuilder.Services.AddSingleton(TimeProvider.System);

webBuilder.Services.AddScoped(srvProvider => new HttpClient
{
    BaseAddress = new Uri(webBuilder.HostEnvironment.BaseAddress)
});

webBuilder.Services.AddTransient<AuthenticationHeaderHandler>();

//webBuilder.Services.AddLogging(logBuilder =>
//{
//    logBuilder.ClearProviders();
//    logBuilder.AddProvider();
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

webBuilder.Services.AddHttpClient(HttpClientName.Api.WithAuthentication, httpClient =>
{
    httpClient.BaseAddress = new Uri(webBuilder.Configuration["Route:Base"]!);
})
    .AddHttpMessageHandler<AuthenticationHeaderHandler>();

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
