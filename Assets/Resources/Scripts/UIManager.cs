using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI authLabel;
    [SerializeField] GameObject joinLobbyButton, authButton;
    [SerializeField] GameObject lobbyContainer, authContainer;
    [SerializeField] UserSlot[] userSlots;



    public void Start()
    {
        GameSingleton.GetInstance<DiscordManager>().authDone += AuthFinished;

    }

    void DisplayLobby()
    {
        authContainer.SetActive(false);
        lobbyContainer.SetActive(true);
        //UpdateLobby();
    }

    void UpdateLobby()
    {

    }

    void AuthFinished()
    {
        var data = GameSingleton.GetInstance<DiscordManager>().currentUserData;
        if (!data.HasValue) return;

        authLabel.text = $"Bonjour {data.Value.userName}!";
        joinLobbyButton.SetActive(true);
        authButton.SetActive(false);

    }

}


[System.Serializable]
public struct UserSlot
{
    public TextMeshProUGUI userNameText;
    public Image userProfilePic;
}
