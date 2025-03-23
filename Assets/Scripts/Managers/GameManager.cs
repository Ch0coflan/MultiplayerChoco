using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Managers
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;
        public GameObject playerPrefab;
        public Transform[] spawnPoints;
        public GameObject loadingScreen;
        [SerializeField] private bool IsAllPlayersReady = false;
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
            Debug.Log("Jugador unido a la sala: " + PhotonNetwork.CurrentRoom.Name);
            PhotonNetwork.LoadLevel(1);
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

        [PunRPC]
        private void SpawnPlayers()
        {
            foreach (Photon.Realtime.Player player in PhotonNetwork.PlayerList)
            {
                if (player.IsLocal)
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
                }
            }
            loadingScreen.SetActive(false);
        }
    }
}