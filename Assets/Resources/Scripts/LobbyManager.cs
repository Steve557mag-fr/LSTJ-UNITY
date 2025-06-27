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
    public WebSocket websocket;

    JObject jsonResponse;
    Dictionary<string, Action<JObject>> responses;
    bool isConnected;

    private void Awake()
    {
        responses = new Dictionary<string, Action<JObject>>()
        {
            {"create_user", OnUserCreated },
            {"create_lobby", OnLobbyCreated },
            {"try_join_lobby", OnTryJoinLobby},
            {"leave_lobby", OnLeaveLobby},
            {"get_lobbies", OnLobbiesFetch},
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

    private void OnLobbiesFetch(JObject response)
    {
        JObject[] lobbies = response["lobbies"].ToObject<JObject[]>();
    }

    private void OnLeaveLobby(JObject response)
    {
        bool left = response["left"].ToObject<bool>();
    }

    private void OnTryJoinLobby(JObject response)
    {
        bool joined = response["joined"].ToObject<bool>();
        string message = response["message"].ToString();
    }

    private void OnLobbyCreated(JObject response)
    {
        string lobbyId = response["lobby_id"].ToString();
    }

    private void OnUserCreated(JObject response) 
    {
        string uuid = response["user_id"].ToString();
        OnLog($"new uuid received : {uuid}", LoggingSeverity.Message);
    }

    //to do refacturisé grace à L'UIManager

    public async void Connect(string username)
    {
        websocket = new WebSocket("ws://localhost:3030");

        websocket.OnOpen += () =>
        {
            ToWSS(new()
            {
                {"request_method", "create_user" },
                {"user_name", username }
            });
            Debug.Log("Connected !");
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

    public void TryJoinLobby()
    {
        //get lobbies;
        //ToWSS(new()
        //{

        //});
        //connect to available lobby;
        //create lobby;
    
    }

    void OnMessage(byte[] bytes)
    {
        string message = System.Text.Encoding.UTF8.GetString(bytes);
        JObject jsonResponse = JObject.Parse(message);

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
        await websocket.Close();
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