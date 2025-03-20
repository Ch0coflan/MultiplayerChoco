using ExitGames.Client.Photon;
using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;    
    public static event Action OnPlayerCollisionWithObstacle;
    public static event Action OnPlayerWin;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }else
        {
            Instance = this;
        }
    }
    public static void TriggerPlayerCollision()
    {
        OnPlayerCollisionWithObstacle?.Invoke();
    }

    public static void TriggerPlayerWin()
    {
        OnPlayerWin?.Invoke();
    }
}
