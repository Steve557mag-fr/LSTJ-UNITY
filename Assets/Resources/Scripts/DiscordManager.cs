using UnityEngine;
using Discord.Sdk;
using System;

public class DiscordManager : GameSingleton
{
    [SerializeField] ulong clientID = 0;
    [SerializeField] ulong appID = 0;

    Client client;
    string codeVerifier;

    string currentToken;
    ulong currentLobby;



    public UserData? currentUserData;

    public delegate void OnDiscordAuthDone();
    public OnDiscordAuthDone authDone;

    public delegate void OnLobbyJoined();
    public OnLobbyJoined lobbyJoined;



    private void Start()
    {
        client = new Client();
        client.AddLogCallback(OnLog, LoggingSeverity.Verbose);

        //client.SetLobbyCreatedCallback();
        //client.SetLobbyUpdatedCallback();

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
        OnLog($"auth result : [{result}][{code}][{redirectUri}]");
        if (!result.Successful()) return;

        client.GetToken(appID, code, codeVerifier, redirectUri, OnTokenExchange);

    }

    void OnTokenExchange(ClientResult result, string accessToken, string refreshToken, AuthorizationTokenType tokenType, int expiresIn, string scopes)
    {
        if(accessToken == "")
        {
            OnLog("token failed!", LoggingSeverity.Warning);
            return;
        }

        currentToken = accessToken;
        client.UpdateToken(tokenType, accessToken, (ClientResult result) => {
            client.Connect();
            authDone.Invoke();
            //client.FetchCurrentUser(tokenType, currentToken, UserDiscordUpdated);

            currentUserData = new() { userName = client.GetCurrentUser().Username(), userId = client.GetCurrentUser().Id() };

        });
    }


    private void UserDiscordUpdated(ClientResult result, ulong id, string name)
    {
        if (!result.Successful()) return;

        currentUserData = new() { userName = name, userId = id };
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

    void OnLog(string message, LoggingSeverity severity = LoggingSeverity.Verbose)
    {
        Debug.Log($"[DISC/{severity}]: {message}");
    }

}

public struct UserData
{
    public string userName;
    public ulong userId;
}
