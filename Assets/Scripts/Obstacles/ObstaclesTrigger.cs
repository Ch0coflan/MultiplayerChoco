using System;
using Managers;
using UnityEngine;

namespace Obstacles
{
    public class ObstaclesTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EventManager.TriggerPlayerCollision();
                Debug.Log($"Jugador {name} colisionando con obstaculo {this.gameObject.name}");
            }
        }
    }
}
