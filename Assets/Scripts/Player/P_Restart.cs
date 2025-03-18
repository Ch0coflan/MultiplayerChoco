using UnityEngine;

public class P_Restart : MonoBehaviour
{
    public Vector3 startPos;

    private void OnEnable()
    {
        EventManager.OnPlayerCollisionWithObstacle += RestartPlayer;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerCollisionWithObstacle -= RestartPlayer;
    }

    
    private void RestartPlayer()
    {
        Debug.Log("Reiniciando al player");
    }
}
