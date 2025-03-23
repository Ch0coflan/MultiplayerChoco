using Photon.Pun;

namespace Player
{
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
}
