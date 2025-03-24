using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Managers
{
    public class GameLoopManager : MonoBehaviour
    {
        public static GameLoopManager Instance;
        private List<GameObject> _playersInGame = new List<GameObject>();
        private int _playersAtDoor = 0;
        private int _totalPlayers;
        
        private void OnEnable()
        {
            EventManager.OnPlayerCollisionWithObstacle += PlayerCollidedWithObstacle;
            EventManager.OnPlayerIsAtDoor += PlayerReachedDoor;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerCollisionWithObstacle -= PlayerCollidedWithObstacle;
            EventManager.OnPlayerIsAtDoor -= PlayerReachedDoor;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void AddPlayer(GameObject player)
        {
            _playersInGame.Add(player);
            _totalPlayers = _playersInGame.Count; 
        }

        public void PlayerReachedDoor(GameObject player)
        {
            _playersAtDoor++;
            Debug.Log($"Jugadores en la puerta: {_playersAtDoor}/{_totalPlayers}");
            if (_playersAtDoor == _totalPlayers)
            {
                TriggerWin();
            }
        }
        public void PlayerCollidedWithObstacle()
        {
            Debug.Log("¡Un jugador tocó un obstáculo! Reiniciando todos los jugadores.");
            ResetAllPlayers();
        }

        private void ResetAllPlayers()
        {
            foreach (var player in _playersInGame)
            {
                player.GetComponent<P_Restart>().RestartPlayer(); 
            }
            _playersAtDoor = 0;
        }

        private void TriggerWin()
        {
            Debug.Log("¡Todos los jugadores han ganado!");
            EventManager.TriggerPlayerWin();
        }
    }
}

