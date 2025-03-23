using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PhotonConnection
{
    public class PartyManager : MonoBehaviourPunCallbacks
    {
        public GameObject lobby;
        public GameObject playerNamePrefab;
        public GameObject playerListContainer;
        public TMP_Text nameRoom;
        public Button startButton;
        private PhotonView _photonViews;
 
        private void Start()
        {
            _photonViews = GetComponent<PhotonView>();
            lobby.SetActive(false);
            startButton.interactable = false;
            UpdatePlayerList();
        }

        public override void OnJoinedRoom()
        {
            Debug.Log($"{PhotonNetwork.NickName} ha entrado a la sala " + PhotonNetwork.CurrentRoom.Name);
            lobby.SetActive (true);
            UpdateNameRoom();
            UpdatePlayerList();
            CheckIfCanStartGame();
        }
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Debug.Log("Nuevo jugador en sala: " + newPlayer.NickName);
            UpdatePlayerList();
            CheckIfCanStartGame();
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            Debug.Log("El jugador " + otherPlayer.NickName + " Ha salido de la sala");
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
                GameObject playerNameObj = Instantiate(playerNamePrefab, playerListContainer.transform, false);

                TextMeshProUGUI playerNameText = playerNameObj.GetComponentInChildren<TextMeshProUGUI>();
                if(playerNameText != null )
                {
                    playerNameText.text = player.NickName;
                }
            }
        }

        private void CheckIfCanStartGame()
        {
            if(PhotonNetwork.CurrentRoom.PlayerCount >= 2 && PhotonNetwork.IsMasterClient)
            {
                startButton.interactable = true;
            }else
            {
                startButton.interactable = false;
            }
        }

        public void OnStartGameButtonClicked()
        {
            if(PhotonNetwork.IsMasterClient && PhotonNetwork.IsConnected)
            {
                Debug.Log("Iniciando juego");
                _photonViews.RPC("StartGameRPC", RpcTarget.All);
            }
            else
            {
                Debug.LogWarning("Solo el Master puede iniciar el juego");
            }
        }

        [PunRPC]
        public void StartGameRPC()
        {
            PhotonNetwork.LoadLevel(1);
        }

        private void UpdateNameRoom()
        {
            string roomName = PhotonNetwork.CurrentRoom.Name;
            nameRoom.text = roomName;
            Debug.Log($"{PhotonNetwork.NickName} has joined to " + roomName);
        }

        public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
        {
            base.OnMasterClientSwitched(newMasterClient);
            Debug.Log("Nuevo Master Client: " + newMasterClient.NickName);
        }
    }
}

