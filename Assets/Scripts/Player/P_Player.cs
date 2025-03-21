using Photon.Pun;
using UnityEngine;

public class P_Player : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(gameObject);
        }
    }
}
