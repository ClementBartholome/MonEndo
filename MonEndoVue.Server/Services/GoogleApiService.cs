using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;

namespace MonEndoVue.Server.Services;

public class GoogleApiService
{
    private string clientId = "744623412588-q46cvhifqpf3ablm4m9tulm3m88odihu.apps.googleusercontent.com";
    private string clientSecret = "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";
    
    public Task<string> GetAuthorizationUrl(string redirectUri)
    {
        // Create the authorization code flow.
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = new string[] { CalendarService.Scope.Calendar }
        });

        // Generate the authorization URL.
        var authorizationUrl = flow.CreateAuthorizationCodeRequest(redirectUri);
        authorizationUrl.RedirectUri = redirectUri;
        return Task.FromResult(authorizationUrl.Build().ToString());
    }
}