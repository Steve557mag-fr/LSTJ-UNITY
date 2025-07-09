using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, username;
    [SerializeField] GameObject authButton;
    [SerializeField] GameObject joinedContainer, lobbyContainer, authContainer, markReadySystem;
    [SerializeField] GameObject check1, check2, check3, check4;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] CanvasGroup authError;
    [SerializeField] UserSlot[] userSlots;

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
        if(usernameInput.text.Length > 3)
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
    private void UpdateLobby()
    {
        // Changer les ready marks hmm hmm;
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
