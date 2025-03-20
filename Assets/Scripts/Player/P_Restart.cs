using UnityEngine;

public class P_Restart : MonoBehaviour
{
    public Transform startPos;
    public GameObject player;

    private void OnEnable()
    {
        player = this.gameObject;
        EventManager.OnPlayerCollisionWithObstacle += RestartPlayer;
    }
    private void OnDisable()
    {
        EventManager.OnPlayerCollisionWithObstacle -= RestartPlayer;
    }

    
    private void RestartPlayer()
    {
        Debug.Log("Reiniciando al player");
        player.transform.position = startPos.position;
    }
}
