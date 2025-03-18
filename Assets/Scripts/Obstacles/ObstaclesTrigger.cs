using System;
using UnityEngine;

public class ObstaclesTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventManager.TriggerPlayerCollision();
            Debug.Log($"Colisionado con {name}");
        }
    }
}
