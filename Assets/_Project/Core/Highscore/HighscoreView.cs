using SpaceChaser.Core.Death;
using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Highscore
{
    public class HighscoreView : MonoBehaviour, IResetable
    {
        private IHighscore _highscore;
        [SerializeField] private LineRenderer _max;
        [SerializeField] private LineRenderer _allTimeMax;
        private Camera _mainCamera;

        [Inject]
        public void Construct(IHighscore highscore)
        {
            _highscore = highscore;
            //_max = GetComponent<LineRenderer>();
            _mainCamera = Camera.main;

            _max.positionCount = 2;
            _allTimeMax.positionCount = 2;
            _max.useWorldSpace = true;
            _allTimeMax.useWorldSpace = true;

            _max.enabled = false;
            _allTimeMax.enabled = false;
        }

        public void Reset()
        {
            if (!_highscore.Available || _mainCamera == null)
            {
                if (_allTimeMax.enabled) _allTimeMax.enabled = false;
                return;
            }

            if (!_allTimeMax.enabled) _allTimeMax.enabled = true;
            Vector2 leftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
            Vector2 rightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane));

            float allTime = Mathf.Max(_highscore.AllTimeHigh, _highscore.High);

            _allTimeMax.SetPosition(0, new Vector3(leftEdge.x, allTime, 0));
            _allTimeMax.SetPosition(1, new Vector3(rightEdge.x, allTime, 0));
        }

        private void LateUpdate()
        {
            if (!_highscore.Available || _mainCamera == null)
            {
                if (_max.enabled) _max.enabled = false;
                return;
            }

            if (!_max.enabled) _max.enabled = true;

            float high = _highscore.High;

            Vector2 leftEdge = _mainCamera.ViewportToWorldPoint(new Vector3(0, 0, _mainCamera.nearClipPlane));
            Vector2 rightEdge = _mainCamera.ViewportToWorldPoint(new Vector3(1, 0, _mainCamera.nearClipPlane));

            _max.SetPosition(0, new Vector3(leftEdge.x, high, 0));
            _max.SetPosition(1, new Vector3(rightEdge.x, high, 0));

            if (_allTimeMax.enabled)
            {
                _allTimeMax.SetPosition(0, new Vector3(leftEdge.x, _highscore.AllTimeHigh, 0));
                _allTimeMax.SetPosition(1, new Vector3(rightEdge.x, _highscore.AllTimeHigh, 0));
            }
        }
    }
}