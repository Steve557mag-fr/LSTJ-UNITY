using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, lobbyPlayersCount, username;
    [SerializeField] GameObject authButton;
    [SerializeField] GameObject joinedContainer, lobbyContainer, authContainer, markReadySystem;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] CanvasGroup authError;
    [SerializeField] UserSlot[] userSlots;

    LobbyManager lobbyManager;

    public void Start()
    {
        lobbyManager = GameSingleton.GetInstance<LobbyManager>();

        lobbyManager.onAuthentificated += AuthFinished;
        lobbyManager.onJoinedLobby += DisplayLobby;

    }

    public void Connect()
    {
        if(usernameInput.text.Length > 3)
        {
        lobbyManager.Connect(usernameInput.text);
        }
    }

    void DisplayLobby()
    {
        joinedContainer.SetActive(false);
        lobbyContainer.SetActive(true);
        markReadySystem.SetActive(true);
        //UpdateLobby();
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
    public GameObject container;
    public TextMeshProUGUI userNameText;
    public Image userProfilePic;
}
