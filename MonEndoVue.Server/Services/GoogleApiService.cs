﻿using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;

namespace MonEndoVue.Server.Services;

public class GoogleApiService
{
    private string clientId = "309720076969-f8emq2flsfap0i4jtrl26f6f1fb8ckmi.apps.googleusercontent.com";
    private string clientSecret = "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";
    
    public async Task<string> GetAuthorizationUrl(string redirectUri)
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
        return authorizationUrl.Build().ToString();
    }
}