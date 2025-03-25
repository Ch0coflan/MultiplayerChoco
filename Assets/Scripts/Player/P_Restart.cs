using System;
using Managers;
using UnityEngine;

namespace Player
{
    public class P_Restart : MonoBehaviour
    {
        public Vector3 startPos;
        public GameObject player;

        private void OnEnable()
        {
            EventManager.OnPlayerCollisionWithObstacle += RestartPlayer;
        }

        private void OnDisable()
        {
            EventManager.OnPlayerCollisionWithObstacle -= RestartPlayer;
        }

        private void Start()
        {
            player = this.gameObject;
            startPos = transform.position;
        }

        public void RestartPlayer()
        {
            Debug.Log("Reiniciando al player");
            player.transform.position = startPos;
        }
    }
}
