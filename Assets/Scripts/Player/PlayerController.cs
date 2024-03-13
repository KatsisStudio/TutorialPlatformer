using System.Linq;
using TutorialPlatformer.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TutorialPlatformer.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Animator), typeof(SpriteRenderer))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody2D _rb;
        private Animator _anim;
        private SpriteRenderer _sr;

        private float _xMov;
        private int _mapLayer;
        private Vector2 _startPos;

        private readonly float[] _raycastPos = new[] { -.6f, .44f };
        private const float RaycastDist = 1f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
            _sr = GetComponent<SpriteRenderer>();

            _startPos = transform.position;
            _mapLayer = LayerMask.NameToLayer("Map");
        }

        private void Update()
        {
            _anim.SetBool("IsJumping", !IsOnFloor());
        }

        private void FixedUpdate()
        {
            _rb.velocity = new(_xMov * _info.Speed, _rb.velocity.y);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                transform.position = _startPos;
            }
        }

        private bool DoesRayHit(float xPos)
        {
            var hit = Physics2D.Raycast(transform.position + Vector3.right * xPos, Vector2.down, RaycastDist, 1 << _mapLayer);

            return hit.collider != null;
        }

        private bool IsOnFloor()
        {
            return _raycastPos.Any(x => DoesRayHit(x));
        }

        public void OnMove(InputAction.CallbackContext value)
        {
            _xMov = value.ReadValue<Vector2>().x;

            if (_xMov < 0f)
            {
                _sr.flipX = true;
                _xMov = -1f;
            }
            else if (_xMov > 0f)
            {
                _sr.flipX = false;
                _xMov = 1f;
            }

            _anim.SetBool("IsRunning", _xMov != 0f);
        }

        public void OnJump(InputAction.CallbackContext value)
        {
            if (value.performed && IsOnFloor())
            {
                _rb.velocity = new(_rb.velocity.x, _info.JumpForce);
            }
        }

        private void OnDrawGizmos()
        {
            foreach (var d in _raycastPos)
            {
                Gizmos.color = DoesRayHit(d) ? Color.green : Color.red;
                var startPos = transform.position + Vector3.right * d;
                Gizmos.DrawLine(startPos, startPos + Vector3.down * RaycastDist);
            }
        }
    }
}