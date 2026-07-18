using SpaceChaser.Core.Inputs;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class PlayerController : MonoBehaviour
    {
        private PlayerConfig _config;
        private IInputReader _inputs;

        private Rigidbody2D _rb;
        private Collider2D _col;

        public float VelocityX
        {
            get => _rb.linearVelocityX;
            set
            {
                _rb.linearVelocityX = value;
            }
        }
        public float VelocityY
        {
            get => _rb.linearVelocityY;
            set
            {
                _rb.linearVelocityY = value;
            }
        }

        [HideInInspector] public float LastJumpTime = float.MinValue;
        [HideInInspector] public float LastGroundedTime { get; private set; } = 0f;
        [field: SerializeField] public bool IsGrounded { get; private set; }
        private bool _isFacingRight = true;

        [Inject]
        public void Construct(PlayerConfig config, IInputReader inputs)
        {
            _config = config;
            _inputs = inputs;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();
        }
        public void Start()
        {
            SetGravity(_config.GravityScale);
        }
        private void FixedUpdate()
        {
            IsGrounded = CheckGrounded();

            if (IsGrounded)
            {
                LastGroundedTime = Time.time;
            }
        }
        public void SetGravity(float scale)
        {
            _rb.gravityScale = scale;
        }

        public Vector2 GetWorldAimPosition()
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(_inputs.CursorScreenPosition);
            return new Vector2(worldPos.x, worldPos.y);
        }

        private bool CheckGrounded()
        {
            Vector2 origin = new(_col.bounds.center.x, _col.bounds.min.y);

            return Physics2D.BoxCast(origin, new(_col.bounds.size.x, _config.GroundCheckSizeY), 0f, Vector2.down, _config.GroundCheckDistance, LayerMask.GetMask("Ground"));
        }
        public void CheckForFlip(float direction)
        {
            if (Mathf.Abs(direction) < 0.01f) return;

            bool inputRight = direction > 0;

            if (inputRight != _isFacingRight)
            {
                _isFacingRight = !_isFacingRight;

                if (_isFacingRight)
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                else
                    transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        private void OnDrawGizmos()
        {
            Vector3 origin = new(_col.bounds.center.x, _col.bounds.min.y);
            Gizmos.DrawWireCube(origin + new Vector3(0f, -_config.GroundCheckSizeY * 0.5f - _config.GroundCheckDistance * 0.5f, 0f), new Vector3(1f, _config.GroundCheckDistance + _config.GroundCheckSizeY, 0f));
        }
    }
}