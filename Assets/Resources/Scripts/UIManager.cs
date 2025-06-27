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
    [SerializeField] UserSlot[] userSlots;

    LobbyManager lobbyManager;

    public void Start()
    {
        lobbyManager = GameSingleton.GetInstance<LobbyManager>();

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



    void AuthFinished()
    {


    }

}


[System.Serializable]
public struct UserSlot
{
    public GameObject container;
    public TextMeshProUGUI userNameText;
    public Image userProfilePic;
}
