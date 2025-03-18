using System;
using UnityEngine;

public static class EventManager 
{
        public static event Action OnPlayerCollisionWithObstacle;
        public static void TriggerPlayerCollision()
        {
            OnPlayerCollisionWithObstacle?.Invoke();  
        }
}
