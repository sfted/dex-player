namespace DexPlayer.Services;

using AdysTech.CredentialManager;
using DexPlayer.Exceptions;
using DexPlayer.Values;
using System.Net;

internal interface IAuthService
{
    public string Token { get; }

    public void LoadToken();
    public void SaveToken(string token);
    public void DeleteToken();
}

// буду рад, если кто-то предложит лучший способ хранения токена.
internal class AuthService : IAuthService
{
    private const string TARGET = $"{AppInfo.APP_NAME}/token";
    private const string USERNAME = "token";

    public string Token { get; private set; }

    public void LoadToken()
    {
        var cred = CredentialManager.GetCredentials(TARGET);

        if (cred != null && !string.IsNullOrEmpty(cred.Password))
            Token = cred.Password;
        else
            throw new TokenNotFoundException();
    }

    public void SaveToken(string token)
    {
        Token = token;
        var cred = new NetworkCredential(USERNAME, token);
        CredentialManager.SaveCredentials(TARGET, cred);
    }

    public void DeleteToken()
    {
        CredentialManager.RemoveCredentials(TARGET);
    }
}
