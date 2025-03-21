using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject playerPrefab;
    public Transform[] spawnPoints;
   /* public bool IsGameStarted()
    {
        return isGameStarted;
    }*/

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    void Start()
    {
         if (PhotonNetwork.IsMasterClient)
        {
           
        }
         SpawnPlayers();
    }

    private void SpawnPlayers()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    /*private bool isGameStarted = false;

    // Método para iniciar la partida
    public void StartGame()
    {
        if (PhotonNetwork.IsMasterClient && !isGameStarted)
        {
            photonView.RPC("StartGameRPC", RpcTarget.All);  // Llama al RPC para que todos los jugadores inicien el juego
            isGameStarted = true;  // Cambia el estado local
        }
    }
        
    [PunRPC]
    public void StartGameRPC()
    {
        isGameStarted = true;  

        // Aquí puedes colocar la lógica que necesite ejecutarse cuando el juego comience, por ejemplo:
        // Cargar la escena del juego, habilitar los controles, etc.
        PhotonNetwork.LoadLevel("GameScene");  // Ejemplo para cargar la escena de juego
    }*/
}
