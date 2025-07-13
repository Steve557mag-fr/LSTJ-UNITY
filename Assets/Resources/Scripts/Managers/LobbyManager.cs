using UnityEngine;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using System;
using System.Collections.Generic;

public class LobbyManager : GameSingleton
{
    //Delegate
    public delegate void OnAuth(bool state = true, string username = "");
    public OnAuth onAuthentificated;
    public delegate void BaseDelagate();
    public BaseDelagate onJoinedLobby;
    public BaseDelagate onLeftLobby;
    public delegate void OnLobbyUpdated(JObject lobbyData);
    public OnLobbyUpdated onLobbyUpdate;

    [SerializeField] UILobby uiLobby;
    public WebSocket websocket;

    private JObject jsonResponse;
    private Dictionary<string, Action<JObject>> responses;
    private bool isConnected;
    private string username;
    private string lobbyId = "";
    public string uuid;

    private void Awake()
    {
        responses = new Dictionary<string, Action<JObject>>()
        {
            {"create_user", OnUserCreated},
            {"join_or_create_lobby", OnJoinedOrCreatedLobby},
            {"leave_lobby", OnLeaveLobby},
            {"get_meta", OnDataFetch},
            {"set_meta", OnDataSet},
            {"send_data", OnSendData},
            {"received_data", OnReceivedData},
            {"lobby_updated", OnLobbyUpdate}
        };
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR 
        if (isConnected)
        {
            websocket.DispatchMessageQueue();
        }
#endif
    }

    private void OnLobbyUpdate(JObject response)
    {
        JObject lobbyData = response["lobby"].ToObject<JObject>();

        onLobbyUpdate(lobbyData);
    }

    private void OnReceivedData(JObject response)
    {
        JObject receivedData = response["data"].ToObject<JObject>();
    }

    private void OnSendData(JObject response)
    {
        bool success = response["success"].ToObject<bool>();
    }

    private void OnDataSet(JObject response)
    {
        bool exist = response["exist"].ToObject<bool>();
        if (exist) OnLog("metadata set successfully", LoggingSeverity.Info);
        else OnLog("ta race", LoggingSeverity.Verbose);
    }

    private void OnDataFetch(JObject response)
    {
        bool exist = response["exist"].ToObject<bool>();
        string val = response["val"].ToString();
    }

    private void OnLeaveLobby(JObject response)
    {
        bool left = response["left"].ToObject<bool>();
        onLeftLobby();
    }

    private void OnJoinedOrCreatedLobby(JObject response)
    {
        bool joined = response["joined"].ToObject<bool>();
        if (joined)
        {
            lobbyId = response["lobby_id"].ToString();
            ToWSS(new()
            {
                {"request_method", "set_meta"},
                {"lobby_id", lobbyId},
                {"key", $"{uuid}_check"},
                {"val", false}
            });
            onJoinedLobby(); 
            OnLog($"Joined Lobby ! Lobby id : {lobbyId}");
        }
        else
        {
            string message = response["message"].ToString();

            OnLog(message, LoggingSeverity.Warning);
        }
    }

    private void OnUserCreated(JObject response) 
    {
        uuid = response["user_id"].ToString();
        onAuthentificated(state: true, username);
        OnLog($"new uuid received : {uuid}", LoggingSeverity.Message);
    }

    public async void Connect(string username)
    {
        websocket = new WebSocket("ws://51.75.121.124:3030");

        websocket.OnOpen += () =>
        {
            ToWSS(new()
            {
                {"request_method", "create_user" },
                {"user_name", username }
            });
            OnLog($"Connected ! Hello {username}", LoggingSeverity.Info);
            this.username = username;
            isConnected = true;
        };

        websocket.OnError += (e) =>
        {
            OnLog($"Error! {e}", LoggingSeverity.Error);
            isConnected = false;
        };

        websocket.OnClose += OnClose;

        websocket.OnMessage += OnMessage;

        // waiting for messages
        await websocket.Connect();
    }

    void ToWSS(JObject jsonRequest)
    {
        websocket.SendText(jsonRequest.ToString());
        OnLog($"Sent Message of method {jsonRequest["request_method"]}", LoggingSeverity.Info);
    }

    public void JoinOrCreateLobby()
    {
        ToWSS(new()
        {
            {"request_method", "join_or_create_lobby" },
            {"user_id", uuid }
        });

    }

    public void Ready(bool state)
    {
        ToWSS(new()
        {
            {"request_method", "set_meta"},
            {"lobby_id", lobbyId},
            {"key", $"{uuid}_check"},
            {"val", state}
        });
    }

    public void LeaveLobby()
    {
        ToWSS(new()
        {
            {"request_method", "leave_lobby" },
            {"user_id", uuid}
        });
        lobbyId = "";
    }

    void OnMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        jsonResponse = JObject.Parse(message);

        OnLog($"Message Received -- Content : {jsonResponse}");

        if (responses.ContainsKey(jsonResponse["request_method"].ToString()))
        {
            responses[jsonResponse["request_method"].ToString()](jsonResponse);
        }
        else
        {
            OnLog("No method was found", LoggingSeverity.Warning);
        }

    }

    void OnClose(WebSocketCloseCode e)
    {
        OnLog($"Connection closed! => {e}", LoggingSeverity.Info);
        isConnected = false;
    }

    private async void OnApplicationQuit()
    {
        if(lobbyId != "")LeaveLobby();
        if(isConnected)await websocket.Close();
        isConnected = false;
    }

    void OnLog(string message, LoggingSeverity severity = LoggingSeverity.Verbose)
    {
        Debug.Log($"[DISC/{severity}]: {message}");
    }

}
public enum LoggingSeverity
{
    Verbose = 1,
    Info = 2,
    Warning = 3,
    Error = 4,
    None = 5,
    Message = 6,
    State = 7,
}