using UnityEngine;
using TMPro;
using Photon.Pun;
using UnityEngine.UI;
using System.Collections;
using Photon.Realtime;

public class MainManager : MonoBehaviourPunCallbacks
{
    [Header("Menu References")]
    public PartyManager partyManager;
    public GameObject menuPanel;
    public GameObject menuCreateOrJoin;
    public GameObject menuJoin;
    public GameObject menuCreate;
    public TMP_InputField CreateRoomNickname;
    public TMP_InputField JoinRoomNickname;
    public TMP_InputField CreateRoomName;
    public TMP_InputField JoinRoomName;
    public Button playButton;
    public Button joinMenuButton;
    public Button createMenuButton;
    public TMP_Text GameTitle;
    public TMP_Text multipurposeText;

    #region NetworkMethods
    private void Start()
    {
        menuPanel.SetActive(true);
        GameTitle.gameObject.SetActive(true);
        menuCreateOrJoin.SetActive(false);
        menuJoin.SetActive(false);
        menuCreate.SetActive(false);
        multipurposeText.gameObject.SetActive(false);
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        OnJoinedLobby();
        Debug.Log("Conected to server!");
    }

    public override void OnJoinedLobby()
    {
        menuCreateOrJoin.SetActive(true);
        multipurposeText.text = string.Empty;
        joinMenuButton.gameObject.SetActive(true); joinMenuButton.interactable = true;
        createMenuButton.gameObject.SetActive(true); createMenuButton.interactable = true;
        Debug.Log("Connected to lobby!");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("Joined to room: " + PhotonNetwork.CurrentRoom.Name);
        if (partyManager != null)
        {
            partyManager.OnJoinedRoom();
        }
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        multipurposeText.text = "Room created successfully";
        Debug.Log("Successfully created room: " + PhotonNetwork.CurrentRoom.Name);
        if (partyManager != null)
        {
            partyManager.OnJoinedRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Debug.Log("Player left the room.");
        multipurposeText.text = "Has salido de la sala.";  
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.LogError("Can't join to the room " + message);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        multipurposeText.gameObject.SetActive(true);
        multipurposeText.text = "Failed to create room. Please try again!";
        menuCreate.SetActive(true) ;
        StartCoroutine(MultipurposeTextCorroutine());
        Debug.Log("Failed to create room" + message);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.Log("Desconectado de Photon: " + cause.ToString());
    }

    #endregion

    #region ButtonMethods

    public void OnClickConnect()
    {
        PhotonNetwork.ConnectUsingSettings();
        menuPanel.SetActive(false);
        multipurposeText.gameObject.SetActive(true);
        multipurposeText.text = "Loading...";
        Debug.Log("Click Connect Done");
    }
    public void JoinButtonMenu()
    {
        menuCreateOrJoin.SetActive(false);
        GameTitle.gameObject.SetActive(false);
        menuJoin.SetActive(true);
    }

    public void CreateButtonMenu()
    {
        menuCreateOrJoin.SetActive(false);
        GameTitle.gameObject.SetActive(false);
        menuCreate.SetActive(true);
    }

    public void OnJoinRoomButtonClicked()
    {
        multipurposeText.gameObject.SetActive(true);
        string jNickname = JoinRoomNickname.text;
        string jRoomName = JoinRoomName.text;

        if (string.IsNullOrEmpty(jNickname) || string.IsNullOrEmpty(jRoomName))
        {
            multipurposeText.text = "Please enter a nickname and a room name!";
            StartCoroutine(MultipurposeTextCorroutine());
            return;
        }

        PhotonNetwork.NickName = jNickname;
        multipurposeText.text = "Joining...";
        StartCoroutine(MultipurposeTextCorroutine());
        menuJoin.SetActive(false);
        PhotonNetwork.JoinRoom(jRoomName);
    }


    public void OnCreateRoomButtonClicked()
    {
        multipurposeText.gameObject.SetActive(true);
        string cNickname = CreateRoomNickname.text;
        string cRoomName = CreateRoomName.text;

        if (string.IsNullOrEmpty(cNickname) || string.IsNullOrEmpty(cRoomName))
        {
            multipurposeText.text = "Please enter a nickname and a room name!";
            StartCoroutine(MultipurposeTextCorroutine());
            return;
        }

        Debug.Log("Creando sala con el nombre: " + cRoomName);
        Debug.Log("El nickname del jugador es: " + cNickname);
        PhotonNetwork.NickName = cNickname;
        multipurposeText.text = "Creating and Joining...";
        StartCoroutine(MultipurposeTextCorroutine());
        menuCreate.SetActive(false);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsOpen= true;
        roomOptions.IsVisible= true;

        PhotonNetwork.CreateRoom(cRoomName, roomOptions, TypedLobby.Default);
    }

    #endregion

    #region BackButtonsMethods

    public void OnChoiceRoomOptionBackButtonPressed()
    {
        menuCreateOrJoin.SetActive(false);
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Desconectando de Photon...");
            PhotonNetwork.Disconnect();
        }
        menuPanel.SetActive(true);
        GameTitle.gameObject.SetActive(true);
    }

    public void OnCreateRoomBackButton()
    {
        menuCreate.SetActive(false);
        menuCreateOrJoin.SetActive(true);
        GameTitle.gameObject.SetActive(true);
    }

    public void OnJoinRoomBackButton()
    {
        menuJoin.SetActive(false);
        menuCreateOrJoin.SetActive(true);
        GameTitle.gameObject.SetActive(true);
    }

    public void LeaveLobbyButton()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leaving the room...");
        partyManager.lobby.SetActive(false);
        menuCreateOrJoin.SetActive(true);
        GameTitle.gameObject.SetActive(true);
        multipurposeText.text = "Saliendo de la sala...";
    }
    public void ExitGameButton()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Desconectando de Photon...");
            PhotonNetwork.Disconnect();
        }
        Debug.Log("Cerrando el juego...");
        Application.Quit();

     #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }


    #endregion

    private IEnumerator MultipurposeTextCorroutine()
    {
        yield return new WaitForSeconds(2);
        multipurposeText.gameObject.SetActive(false);
    }
}

    
