using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            EventManager.TriggerPlayerWin();
            Debug.Log("Colisionando con lapuerta");
        }
    }
}
