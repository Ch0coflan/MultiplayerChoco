using ExitGames.Client.Photon;
using System;
using UnityEngine;

public static class EventManager 
{
        public static event Action OnPlayerCollisionWithObstacle;
    public static event Action OnPlayerWin;
        public static void TriggerPlayerCollision()
        {
            OnPlayerCollisionWithObstacle?.Invoke();  
        }

    public static void TriggerPlayerWin()
    {

    }
}
