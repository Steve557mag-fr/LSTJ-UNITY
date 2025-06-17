using UnityEngine;
using Discord.Sdk;

public class DiscordManager : GameSingleton
{
    [SerializeField] ulong clientID = 0;
    [SerializeField] ulong appID = 0;

    Client client;
    string codeVerifier;

    private void Start()
    {
        client = new Client();
        client.AddLogCallback(OnLog, LoggingSeverity.Verbose);
    }

    public void AuthAccount()
    {
        AuthorizationCodeVerifier verifier = client.CreateAuthorizationCodeVerifier();
        codeVerifier = verifier.Verifier();

        AuthorizationArgs authArgs = new AuthorizationArgs();
        authArgs.SetClientId(clientID);
        authArgs.SetScopes(Client.GetDefaultPresenceScopes());
        authArgs.SetCodeChallenge(verifier.Challenge());

        client.Authorize(authArgs, OnAuth);
    }

    void OnAuth(ClientResult result, string code, string redirectUri)
    {
        Debug.Log($"Authorization result: [{result.Error()}] [{code}] [{redirectUri}]");
        if (!result.Successful())
        {
            return;
        }

        client.
        client.GetToken(appID, code, codeVerifier, redirectUri, OnTokenExchange);
    }

    void OnTokenExchange(ClientResult result, string accessToken, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
    {
        if(accessToken == "")
        {
            OnLog("token failed!", LoggingSeverity.Warning);
            return;
        }

        Debug.Log("Token received: " + accessToken);
        client.UpdateToken(AuthorizationTokenType.Bearer, accessToken, (ClientResult result) => { client.Connect(); });
    }


    void OnLog(string message, LoggingSeverity severity)
    {
        Debug.Log($"[DISC/{severity}]: {message}");

    }

}
