using UnityEngine;
using UnityEngine.UI;

public class P_NameCanvas : MonoBehaviour
{
    public Canvas PlayerCanvas;

    private void OnEnable()
    {
        PlayerCanvas = GetComponentInChildren<Canvas>();
    }

    private void Update()
    {
        PlayerCanvas.transform.rotation = Quaternion.identity;
    }
}
