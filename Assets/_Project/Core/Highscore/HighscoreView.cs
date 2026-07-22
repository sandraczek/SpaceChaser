using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Highscore
{
    [RequireComponent(typeof(LineRenderer))]
    public class HighscoreView : MonoBehaviour
    {
        private IHighscore _highscore;
        private LineRenderer _line;
        private Camera _mainCamera;

        [Inject]
        public void Construct(IHighscore highscore)
        {
            _highscore = highscore;
            _line = GetComponent<LineRenderer>();
            _mainCamera = Camera.main;

            _line.positionCount = 2;
            _line.useWorldSpace = true;

            _line.enabled = false;
        }

        private void LateUpdate()
        {
            if (!_highscore.Available || _mainCamera == null)
            {
                if (_line.enabled) _line.enabled = false;
                return;
            }

            if (!_line.enabled) _line.enabled = true;

            float high = _highscore.High;

            Vector2 leftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
            Vector2 rightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane));

            _line.SetPosition(0, new Vector3(leftEdge.x, high, 0));
            _line.SetPosition(1, new Vector3(rightEdge.x, high, 0));
        }
    }
}