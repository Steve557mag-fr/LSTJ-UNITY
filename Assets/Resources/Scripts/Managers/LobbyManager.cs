using UnityEngine;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using System;
using System.Collections.Generic;

public class LobbyManager : GameSingleton
{
    //Delegate
    public delegate void OnAuth(bool success, string username);
    public OnAuth onAuthentificated;
    public delegate void OnJoined();
    public OnJoined onJoinedLobby;

    [SerializeField] UIManager uIManager;
    public WebSocket websocket;

    private JObject jsonResponse;
    private Dictionary<string, Action<JObject>> responses;
    private bool isConnected;
    private string username;
    private string uuid;
    private string lobbyId = "";

    public LoggingSeverity loggingSeverityLevel = LoggingSeverity.Verbose;

    private void Awake()
    {
        responses = new Dictionary<string, Action<JObject>>()
        {
            {"create_user", OnUserCreated },
            {"join_or_create_lobby", OnJoinedOrCreatedLobby},
            {"leave_lobby", OnLeaveLobby},
            {"get_data", OnDataFetch},
            {"set_data", OnDataSet},
            {"lobby_updated", OnLobbyUpdate},
            {"received_data", OnDataReceived }
        };
    }

    public void SendData(JObject data)
    {
        ToWSS(new(){
            {"request_method", "send_data"},
            {"lobby_id", lobbyId },
            {"data", data }
        });
    }

    private void OnDataReceived(JObject packet)
    {
        GameSingleton.GetInstance<MinigamesManager>().GetCurrentMG().ReceiveSocketPacket(packet);
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
    }

    private void OnDataSet(JObject response)
    {
        bool exist = response["exist"].ToObject<bool>();
    }

    private void OnDataFetch(JObject response)
    {
        bool exist = response["exist"].ToObject<bool>();
        string val = response["val"].ToString();
    }

    private void OnLeaveLobby(JObject response)
    {
        bool left = response["left"].ToObject<bool>();
    }

    private void OnJoinedOrCreatedLobby(JObject response)
    {
        bool joined = response["joined"].ToObject<bool>();
        if (joined)
        {
            lobbyId = response["lobby_id"].ToString();
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
        onAuthentificated(success: true, username);
        OnLog($"new uuid received : {uuid}", LoggingSeverity.Message);
    }

    //to do refacturisé grace à L'UIManager

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
        if(severity <= loggingSeverityLevel) Debug.Log($"[DISC/{severity}]: {message}");
    }

}
public enum LoggingSeverity
{
    Error = 1,
    Warning = 2,
    Message = 3,
    State = 4,
    Info = 5,
    Verbose = 6,
    None = 7,
}