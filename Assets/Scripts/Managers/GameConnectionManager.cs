using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameConnectionManager : MonoBehaviourPunCallbacks
    {
        public static GameConnectionManager Instance;
        public GameObject playerPrefab;
        public Transform[] spawnPoints;
        public GameObject loadingScreen;
        [SerializeField] private bool IsAllPlayersReady = false;
        private Dictionary<int, GameObject> playerObjects = new Dictionary<int, GameObject>();
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }


        private void Start()
        {
            if (PhotonNetwork.IsConnected)
            {
                loadingScreen.SetActive(true);
                Debug.Log("Nuevo jugador conectado: " + PhotonNetwork.LocalPlayer.NickName);
            }
        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient && !IsAllPlayersReady)
            {
                CheckAllPlayersInScene();
            }
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            Debug.Log("Jugador unido a la sala: " + PhotonNetwork.CurrentRoom.Name + PhotonNetwork.LocalPlayer.ActorNumber);
            GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity, 0);
            playerObjects.Add(PhotonNetwork.LocalPlayer.ActorNumber, playerPrefab); 
            GameLoopManager.Instance.AddPlayer(player);
        }

        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Debug.Log("Jugador desconectado de la sala: " + otherPlayer.NickName);
            if(playerObjects.ContainsKey(otherPlayer.ActorNumber))
            {
                Destroy(playerObjects[otherPlayer.ActorNumber]);
                playerObjects.Remove(otherPlayer.ActorNumber);
            }

            if (PhotonNetwork.IsMasterClient && otherPlayer.IsMasterClient)
            {
                PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[0]);
            }
           
        }
        private void CheckAllPlayersInScene()
        {
            if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.PlayerCount)
            {
                Debug.Log("Todos los jugadores están en la escena.");
                IsAllPlayersReady = true;
                StartGame();
            }
            else
            {
                Debug.Log("Esperando más jugadores para unirse...");
            }
        }
        
        public void StartGame()
        {
            loadingScreen.SetActive(false);
            Debug.Log("Starting game...");
            photonView.RPC("SpawnPlayers", RpcTarget.All);
        }
        
        private void AssignPlayerName(GameObject playerObject)
        {
            PhotonView photonViewPlayer = playerObject.GetComponent<PhotonView>();
            if (photonViewPlayer == null)
            {
                Debug.LogError("El prefab del jugador no tiene un PhotonView.");
                return;
            }

            TMP_Text playerNameText = playerObject.GetComponentInChildren<TMP_Text>();

            if (playerNameText != null)
            {
                if (photonView.IsMine)
                {
                    playerNameText.text = photonViewPlayer.Owner.NickName;
                    Debug.Log($"Nombre asignado: {photonViewPlayer.Owner.NickName}");
                }
                else
                {
                    photonView.RPC("UpdatePlayerName", RpcTarget.AllBuffered, photonViewPlayer.Owner.NickName);
                }
            }
            else
            {
                Debug.LogError("No se encontró el componente TMP_Text en el prefab del jugador");
            }
        }

        [PunRPC]
        private void SpawnPlayers()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.IsLocal)
                {
                    GameObject playerPrefabs = PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                    AssignPlayerName(playerPrefabs);
                }
            }
            loadingScreen.SetActive(false);
        }

        [PunRPC]
        public void UpdatePlayerName(string playerName)
        {
            TMP_Text playerNameText = GetComponentInChildren<TMP_Text>();
            if (playerNameText != null)
            {
                playerNameText.text = playerName;
            }
        }
    }
}