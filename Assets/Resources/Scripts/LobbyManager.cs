using UnityEngine;
using System.Linq;
using Newtonsoft.Json.Linq;
using NativeWebSocket;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using UnityEditor;

public class LobbyManager : GameSingleton
{
    //Delegate
    public delegate void OnAuth(bool success, string username);
    public OnAuth onAuthentificated;

    [SerializeField] UIManager uIManager;
    public WebSocket websocket;

    private JObject jsonResponse;
    private Dictionary<string, Action<JObject>> responses;
    private bool isConnected;
    private string username;
    private string uuid;

    private void Awake()
    {
        responses = new Dictionary<string, Action<JObject>>()
        {
            {"create_user", OnUserCreated },
            {"join_or_create_lobby", OnJoinedOrCreatedLobby},
            {"leave_lobby", OnLeaveLobby},
            {"get_data", OnDataFetch},
            {"set_data", OnDataSet}
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
                {"user_id", username }
            });
            Debug.Log("Connected !");
            this.username = username;
            isConnected = true;
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
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
        //get lobbies;
        ToWSS(new()
        {
            {"request_method", "join_or_create_lobby" },
            {"user_name", uuid }
        });

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
        OnLog($"Connection closed! => {e}", LoggingSeverity.Error);
        isConnected = false;
    }

    private async void OnApplicationQuit()
    {
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