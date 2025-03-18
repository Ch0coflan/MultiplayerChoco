using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartyManager : MonoBehaviourPunCallbacks
{
    public GameObject lobby;
    public GameObject playerNamePrefab;
    public GameObject playerListContainer;
    public TMP_Text nameRoom;
    public Button startButton;
 
    private void Start()
    {
        lobby.SetActive(false);
        startButton.interactable = false;
        UpdatePlayerList();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Ha entrado a la sala");
        lobby.SetActive (true);
        UpdateNameRoom();
        UpdatePlayerList();
        CheckIfCanStartGame();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdatePlayerList();
        CheckIfCanStartGame();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerList();
        CheckIfCanStartGame();
    }

    private void UpdatePlayerList()
    {
       foreach(Transform child in playerListContainer.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(var player in PhotonNetwork.PlayerList)
        {
            GameObject playerNameObj = Instantiate(playerNamePrefab);
            playerNameObj.transform.SetParent(playerListContainer.transform, false);

            Text playerNameText = playerNameObj.GetComponentInChildren<Text>();
            if(playerNameText != null )
            {
                playerNameText.text = player.NickName;
            }
        }
    }

    private void CheckIfCanStartGame()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startButton.interactable = true;
        }else
        {
            startButton.interactable=false;
        }
    }

    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.LoadLevel(1);
    }

    private void UpdateNameRoom()
    {
        string roomName = PhotonNetwork.CurrentRoom.Name;
        nameRoom.text = roomName;
        Debug.Log("Player has joined to " + roomName);
    }
}

