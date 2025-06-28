using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, lobbyPlayersCount;
    [SerializeField] GameObject joinLobbyButton, authButton;
    [SerializeField] GameObject lobbyContainer, authContainer;
    [SerializeField] TMP_InputField usernameInput;
    [SerializeField] CanvasGroup authError;
    [SerializeField] UserSlot[] userSlots;

    LobbyManager lobbyManager;

    public void Start()
    {
        lobbyManager = GameSingleton.GetInstance<LobbyManager>();

        lobbyManager.onAuthentificated += AuthFinished;

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
        authContainer.SetActive(false);
        lobbyContainer.SetActive(true);
        //UpdateLobby();
    }



    void AuthFinished(bool success, string username)
    {
        if (success)
        {
            authContainer.SetActive(false);
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
