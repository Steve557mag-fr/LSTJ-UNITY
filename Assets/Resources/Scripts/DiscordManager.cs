using UnityEngine;
using Discord.Sdk;
using System;

public class DiscordManager : GameSingleton
{
    [SerializeField] ulong clientID = 0;
    [SerializeField] ulong appID = 0;

    Client client;
    string codeVerifier;

    string mainToken;
    ulong currentLobby;

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
        mainToken = accessToken;

        client.CreateOrJoinLobby("secret_lobby", OnJoinedLobby);

    }

    private void OnJoinedLobby(ClientResult result, ulong lobbyId)
    {
        OnLog("lobby joined!", LoggingSeverity.Info);
        client.SendActivityInvite(622484016066461727, "bouh", OnActivityInvited);
    }

    private void OnActivityInvited(ClientResult result)
    {
        print("huh");
    }

    void OnLog(string message, LoggingSeverity severity)
    {
        Debug.Log($"[DISC/{severity}]: {message}");

    }

}
