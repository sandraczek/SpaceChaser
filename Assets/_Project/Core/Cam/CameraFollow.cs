using SpaceChaser.Core.Death;
using SpaceChaser.Core.Highscore;
using SpaceChaser.Core.Player;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Cam
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollow : MonoBehaviour
    {
        private IPlayerProvider _player;
        private IHighscore _highscore;
        private GameConfig _config;

        private Transform _target;
        public float SmoothTime = 0.3f;

        private Vector3 _velocity = Vector3.zero;
        private Camera _cam;

        [Inject]
        public void Construct(IPlayerProvider player, GameConfig config, IHighscore highscore)
        {
            _player = player;
            _config = config;

            _highscore = highscore;

            _cam = GetComponent<Camera>();
        }
        private void OnEnable()
        {
            _player.OnPlayerRegistered += SetPlayerTarget;
            _player.OnPlayerUnregistered += ClearTarget;
        }
        private void OnDisable()
        {
            if (_player == null) return;
            _player.OnPlayerRegistered -= SetPlayerTarget;
            _player.OnPlayerUnregistered -= ClearTarget;
        }

        private void LateUpdate()
        {
            if (_target == null) return;

            float camHalfHeight = _cam.orthographicSize;
            float camHalfWidth = camHalfHeight * _cam.aspect;

            Vector3 targetPos = _target.position;

            float clampedX = Mathf.Clamp(targetPos.x, _config.MinBounds.x + camHalfWidth, _config.MaxBounds.x - camHalfWidth);

            float yBorder = Mathf.Max(_config.MinBounds.y, _highscore.High - _config.DeathDistance + 2f);
            float clampedY = Mathf.Clamp(targetPos.y, yBorder + camHalfHeight, _config.MaxBounds.y - camHalfHeight);

            Vector3 boundedTarget = new(clampedX, clampedY, transform.position.z);

            transform.position = Vector3.SmoothDamp(
                current: transform.position,
                target: boundedTarget,
                currentVelocity: ref _velocity,
                smoothTime: SmoothTime
            );
        }

        private void SetPlayerTarget()
        {
            _target = _player.Transform;
        }
        private void ClearTarget()
        {
            _target = null;
        }
    }
}