using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using UI;
using UI.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<MovieService>();
builder.Services.AddBlazorBootstrap();

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

builder.Services.AddOidcAuthentication(options => {
    options.ProviderOptions.Authority = "https://dev-fbr2yb3xvzwpcqbi.us.auth0.com";
    options.ProviderOptions.ClientId = "kxUWP45mLLskPwuE6aUKTMlGbaF5rgPo";
    options.ProviderOptions.ResponseType = "code";
    options.ProviderOptions.RedirectUri = $"{builder.HostEnvironment.BaseAddress}authentication/login-callback";
    var logoutUri = new Uri(builder.HostEnvironment.BaseAddress + "authentication/logout-callback");
    options.ProviderOptions.PostLogoutRedirectUri = logoutUri.ToString();

    options.ProviderOptions.DefaultScopes.Clear();
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");

    options.ProviderOptions.AdditionalProviderParameters["audience"] = "https://dev-fbr2yb3xvzwpcqbi.us.auth0.com/api/v2/";

    options.ProviderOptions.AdditionalProviderParameters.Add("returnTo", logoutUri.ToString());
});

await builder.Build().RunAsync();