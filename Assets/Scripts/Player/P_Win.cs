using UnityEngine;

public class P_Win : MonoBehaviour
{
    private void OnEnable()
    {
        EventManager.OnPlayerWin += PlayerWin;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerWin -= PlayerWin;
    }

    private void PlayerWin()
    {

    }
}
