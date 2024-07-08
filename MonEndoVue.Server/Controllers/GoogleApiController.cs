using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Web;
using Google.Apis.Calendar.v3;
using Microsoft.AspNetCore.Mvc;
using MonEndoVue.Server.Services;

namespace MonEndoVue.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class GoogleApiController : ControllerBase
{
    private string clientId = "309720076969-f8emq2flsfap0i4jtrl26f6f1fb8ckmi.apps.googleusercontent.com";
    private string clientSecret = "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";
    
    [HttpGet("authorize")]
    public async Task<IActionResult> Authorize(string code, string state)
    {
        var userId = state;

        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = new string[] { CalendarService.Scope.Calendar }
        });

        var redirectUri = Request.Scheme + "://" + Request.Host.Value + Url.Action("Authorize", "GoogleApi");

        var tokenResponse = await flow.ExchangeCodeForTokenAsync(userId, code, redirectUri, CancellationToken.None);
        var refreshToken = tokenResponse.RefreshToken;

        var redirectUrl = $"https://localhost:7206/?accessToken={tokenResponse.AccessToken}&refreshToken={refreshToken}";

        return Redirect(redirectUrl);
    }
    
    [HttpGet("authenticate")]
    public IActionResult Authenticate(string userId)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = new string[] { CalendarService.Scope.Calendar }
        });

        var uri = Request.Scheme + "://" + Request.Host.Value + Url.Action("Authorize", "GoogleApi");

        var codeRequestUrl = flow.CreateAuthorizationCodeRequest(uri);
        codeRequestUrl.State = userId; 
        var authorizationUrl = codeRequestUrl.Build();

        return Ok(authorizationUrl);
    }
    
    [HttpGet("refreshToken")]
    public async Task<IActionResult> RefreshToken(string userId, string refreshToken)
    {
        var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
        {
            ClientSecrets = new ClientSecrets
            {
                ClientId = clientId,
                ClientSecret = clientSecret
            },
            Scopes = new string[] { CalendarService.Scope.Calendar }
        });

        var response = await flow.RefreshTokenAsync(userId, refreshToken, CancellationToken.None);
        var token = response.AccessToken;

        return Ok(token);
    }
}