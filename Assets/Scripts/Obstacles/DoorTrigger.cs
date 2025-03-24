using Managers;
using UnityEngine;

namespace Obstacles
{
    public class DoorTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.CompareTag("Player"))
            {
                EventManager.TriggerPlayerIsAtDoor(other.gameObject);
                Debug.Log($"El jugador {name} ha llegado a la puerta");
            }
        }
    }
}
