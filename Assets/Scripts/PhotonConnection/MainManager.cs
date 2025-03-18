using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Pun.Demo.Cockpit;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainManager : MonoBehaviourPunCallbacks
{
    [Header("Menu References")]
    public PartyManager partyManager;
    public GameObject menuPanel;
    public GameObject menuCreateOrJoin;
    public GameObject menuJoin;
    public GameObject menuCreate;
    public TMP_InputField nickCreateRoomInput;
    public TMP_InputField nickJoinRoomInput;
    public TMP_InputField roomNameCreateInput;
    public TMP_InputField roomNameJoinInput;
    public Button playButton;
    public Button joinMenuButton;
    public Button createMenuButton;
    public TMP_Text multipurposeText;
   
    #region NetworkMethods
    private void Start()
    {
        menuPanel.SetActive(true);
        menuCreateOrJoin.SetActive(false);
        menuJoin.SetActive(false);
        menuCreate.SetActive(false);
        multipurposeText.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        OnJoinedLobby();
        Debug.Log("Conected to server!");
    }

    public override void OnJoinedLobby()
    {
        menuCreateOrJoin.SetActive(true);
        joinMenuButton.gameObject.SetActive(true); joinMenuButton.interactable = true;
        createMenuButton.gameObject.SetActive(true); createMenuButton.interactable = true;
        Debug.Log("Connected to lobby!");
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined to room: " + PhotonNetwork.CurrentRoom.Name);
        if (partyManager != null)
        {
            partyManager.OnJoinedRoom();
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError("Can't join to the room " + message);
    }


    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            multipurposeText.text = "There's enough players to start!";
            StartCoroutine(MultipurposeTextCorroutine());
        }
    }

    #endregion

    #region ButtonMethods
    public void JoinButtonMenu()
    {
        menuCreateOrJoin.SetActive(false);
        menuJoin.SetActive(true);
    }

    public void CreateButtonMenu()
    {
        menuCreateOrJoin.SetActive(false);
        menuCreate.SetActive(true);
    }

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        menuPanel.SetActive(false);
        multipurposeText.gameObject.SetActive(true);
        Debug.Log("Click Connect Done");
    }

    public void OnJoinRoomButtonClicked()
    {
        multipurposeText.gameObject.SetActive(true);
        string nickname = nickJoinRoomInput.text;
        string roomName = roomNameJoinInput.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            multipurposeText.text = "Please enter a nickname and a room name!";
            StartCoroutine(MultipurposeTextCorroutine());
            return;
        }
        else
        {
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.JoinRoom(roomName);
            multipurposeText.text = "Joining...";
            StartCoroutine(MultipurposeTextCorroutine());
            menuJoin.SetActive(false);
        }
    }

    public void OnCreateRoomButtonClicked()
    {
        multipurposeText.gameObject.SetActive(true);
        string nickname = nickCreateRoomInput.text;
        string roomName = roomNameCreateInput.text;

        if (string.IsNullOrEmpty(nickname) || string.IsNullOrEmpty(roomName))
        {
            multipurposeText.text = "Please enter a nickname and a room name!";
            StartCoroutine(MultipurposeTextCorroutine());
            return;
        }
        else
        {
            PhotonNetwork.NickName = nickname;
            PhotonNetwork.CreateRoom(roomName, new Photon.Realtime.RoomOptions { MaxPlayers = 4 });
            multipurposeText.text = "Creating and Joining...";
            StartCoroutine(MultipurposeTextCorroutine());
            menuCreate.SetActive(false);
        }
    }
    #endregion

    private IEnumerator MultipurposeTextCorroutine()
    {
        yield return new WaitForSeconds(2);
        multipurposeText.gameObject.SetActive(false);
    }
}

    
