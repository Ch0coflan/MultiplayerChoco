using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;    
        public static event Action OnPlayerCollisionWithObstacle;
        public static event Action OnPlayerWin;
        public static event Action<GameObject> OnPlayerIsAtDoor;

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

        public static void TriggerPlayerIsAtDoor(GameObject player)
        {
            OnPlayerIsAtDoor?.Invoke(player);
        }
    
    }
}
