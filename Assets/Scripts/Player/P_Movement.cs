using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class P_Movement : MonoBehaviourPunCallbacks
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 6f;
    private Rigidbody _rb;
    private Transform playerTransform; 
    private Vector3 moveDirection;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        playerTransform = GetComponent<Transform>();
    }
    void Update()
    {
        if(photonView.IsMine)
        {
            InputMovement();
            Move();
            if (moveDirection.y >= 1)
            {
                Fly();
            }
            if(moveDirection.x >= 1)
            {
                playerTransform.rotation = Quaternion.Euler(0,0,0);
            }else if(moveDirection.x <= -1 )
            {
                playerTransform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    private void InputMovement()
    {
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Jump");
        moveDirection *= speed;
    }

    private void Move()
    {
        _rb.linearVelocity = new Vector3(moveDirection.x, _rb.linearVelocity.y,0);
    }

    private void Fly()
    {
        _rb.AddForce(transform.up * jumpForce);
    }
}