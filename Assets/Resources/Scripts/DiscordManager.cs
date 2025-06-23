using UnityEngine;
using Discord.Sdk;
using System;

public class DiscordManager : GameSingleton
{
    [SerializeField] ulong clientID = 0;
    [SerializeField] ulong appID = 0;
    [SerializeField] int maxLobbySize;

    Client client;
    string codeVerifier;

    string currentToken;
    ulong currentLobby;


    public UserData? currentUserData;

    public delegate void OnDiscordAuthDone();
    public OnDiscordAuthDone authDone;

    public delegate void OnLobbyJoined();
    public OnLobbyJoined lobbyJoined;

    private void Awake()
    {
        MakeInstance<DiscordManager>();
    }

    private void Start()
    {
        client = new Client();
        client.AddLogCallback(OnLog, LoggingSeverity.Verbose);
        client.SetActivityInviteCreatedCallback(OnActivityCreated);
        client.SetLobbyCreatedCallback(OnLobbyCreated);

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
            client.FetchCurrentUser(tokenType, currentToken, UserDiscordUpdated);
            OnLog($"Token received : {accessToken}", LoggingSeverity.Info);
        });
    }


    private void UserDiscordUpdated(ClientResult result, ulong id, string name)
    {
        if (!result.Successful()) return;

        currentUserData = new() { userName = name, userId = id };
        authDone.Invoke();
    }

    public void TryJoinLobby()
    {
        ulong[] ids = client.GetLobbyIds();
        OnLog($"Available Lobbies : {ids.Length}", LoggingSeverity.Info);
        if(ids.Length == 0)
        {
            string secret = System.Guid.NewGuid().ToString();
            //client.CreateOrJoinLobbyWithMetadata(System.Guid.NewGuid().ToString(), new() { {"host_id", clientID.ToString() } }, new(), OnJoinedLobby);
            client.CreateOrJoinLobby(secret, OnJoinedLobby);
            OnLog($"Created a new lobby with secret : {secret}", LoggingSeverity.Info);
        }
        foreach(ulong id in ids)
        {
            LobbyHandle lobbyHandle = client.GetLobbyHandle(id);
            ulong hostId = ulong.Parse(lobbyHandle.Metadata()["host_id"]);

            ulong[] membersIds = lobbyHandle.LobbyMemberIds();
            if(membersIds.Length >= maxLobbySize)
            {
                continue;
            }
            else
            {
                client.SendActivityJoinRequest(hostId, (ClientResult result)=>{});
                break;
            }

        }

    }

    private void OnJoinedLobby(ClientResult result, ulong lobbyId)
    {
        OnLog($"lobby joined! -- lobby id : {lobbyId} === Client result : {result}", LoggingSeverity.Info);
    }

    private void OnLobbyCreated(ulong lobbyid)
    {
        OnLog("Lobby Created -- lobby id :" + lobbyid);
    }

    private void OnActivityInvited(ClientResult result)
    {
        print("huh");
    }

    private void OnActivityCreated(ActivityInvite invite)
    {
        client.AcceptActivityInvite(invite, (ClientResult result, string secret) => {
            client.CreateOrJoinLobby(secret, OnJoinedLobby);
        });
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
