using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel, lobbyPlayersCount;
    [SerializeField] GameObject joinLobbyButton, authButton;
    [SerializeField] GameObject lobbyContainer, authContainer;
    [SerializeField] UserSlot[] userSlots;



    public void Start()
    {


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
