using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Newtonsoft.Json.Linq;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, username, readyButton;
    [SerializeField] GameObject authButton;
    [SerializeField] GameObject joinedContainer, lobbyContainer, authContainer, markReadySystem;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] CanvasGroup authError;
    [SerializeField] UserSlot[] userSlots;
    private bool playerIsReady;

    LobbyManager lobbyManager; 

    public void Start()
    {
        lobbyManager = GameSingleton.GetInstance<LobbyManager>();

        lobbyManager.onAuthentificated += AuthFinished;
        lobbyManager.onJoinedLobby += DisplayLobby;
        lobbyManager.onLeftLobby += QuitLobby;
        lobbyManager.onLobbyUpdate += UpdateLobby;

    }


    public void Connect()
    {
        if(usernameInput.text.Length > 1)
        {
            lobbyManager.Connect(usernameInput.text);
        }
    }

    private void QuitLobby()
    {
        lobbyContainer.SetActive(false);
        markReadySystem.SetActive(false);
        joinedContainer.SetActive(true);
    }

    void DisplayLobby()
    {
        joinedContainer.SetActive(false);
        lobbyContainer.SetActive(true);
        markReadySystem.SetActive(true);
    }
    private void UpdateLobby(JObject lobbyData)
    {
        int i = 0;
        foreach(JProperty user in lobbyData["users"])
        {
            string name = user.Value["name"].ToString();
            if (lobbyData["metadata"][$"{user.Name}_check"] == null) continue; 
            bool ready = lobbyData["metadata"][$"{user.Name}_check"].ToObject<bool>();
            userSlots[i].readyMark.SetActive(ready);
            userSlots[i].userNameText.text = name;
            i++;
        }
        if (lobbyData["metadata"][$"{lobbyManager.uuid}_check"] != null)  playerIsReady = lobbyData["metadata"][$"{lobbyManager.uuid}_check"].ToObject<bool>();
    }

    void AuthFinished(bool success, string username)
    {
        if (success)
        {
            authContainer.SetActive(false);
            joinedContainer.SetActive(true);
            this.username.text = $"Hello {username} !";
        }
        else
        {
            LeanLog(authError, 1, 2);
        }

    }

    public void OnReady()
    {
        lobbyManager.Ready(!playerIsReady);
        if (playerIsReady) 
        {
            readyButton.text = "PAS PRÊT";
        }
        else
        {
            readyButton.text = "PRÊT";
        }
    }

    void LeanLog(CanvasGroup text, float alpha, float time, float delay = 3)
    {
        text.LeanAlpha(alpha, time).setOnComplete(() =>
        {
            
            text.LeanAlpha(0, time).setDelay(delay);
        });
    }

}


[System.Serializable]
public struct UserSlot
{
    public GameObject readyMark;
    public TextMeshProUGUI userNameText;
}
