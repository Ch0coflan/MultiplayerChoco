using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class P_Movement : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private float speed = 5f;
        [SerializeField] private float jumpForce = 6f;
        private Rigidbody _rb;
        private Transform _playerTransform; 
        private Vector3 _moveDirection;
        private Vector3 _networkPosition;
        private Quaternion _networkRotation;
      
       void Awake()
       {
           _rb = GetComponent<Rigidbody>();
           _playerTransform = GetComponent<Transform>();
       }

       private void Start()
        {
            if(!photonView.IsMine)
            {
                GetComponent<P_Movement>().enabled = false;
            }
        }
        void Update()
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if(photonView.IsMine)
            {
                InputMovement();
                Move();
                if (_moveDirection.y >= 1)
                {
                    Fly();
                }
                if(_moveDirection.x >= 1)
                {
                    _playerTransform.rotation = Quaternion.Euler(0,90,0);
                }else if(_moveDirection.x <= -1 )
                {
                    _playerTransform.rotation = Quaternion.Euler(0, 270, 0);
                }
                
            }
            else
            {
                _rb.position = Vector3.Lerp(_rb.position, _networkPosition, Time.deltaTime * 5);
                _rb.rotation = Quaternion.Lerp(_rb.rotation, _networkRotation, Time.deltaTime * 5);
            }
        }

        private void InputMovement()
        {
            _moveDirection.x = Input.GetAxisRaw("Horizontal");
            _moveDirection.y = Input.GetAxisRaw("Jump");
            _moveDirection *= speed;
        }

        private void Move()
        {
            _rb.linearVelocity = new Vector3(_moveDirection.x, _rb.linearVelocity.y,0);
        }

        private void Fly()
        {
            _rb.AddForce(transform.up * jumpForce);
        }
        
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(_rb.position);
                stream.SendNext(_rb.rotation);
            }
            else
            {
                _networkPosition = (Vector3) stream.ReceiveNext();
                _networkRotation = (Quaternion) stream.ReceiveNext();
            }
        }
        
    }
}