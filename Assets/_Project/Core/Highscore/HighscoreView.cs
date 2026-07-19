using UnityEngine;
using VContainer;

namespace SpaceChaser.Core.Highscore
{
    [RequireComponent(typeof(LineRenderer))]
    public class HighscoreView : MonoBehaviour
    {
        private IHighscore _highscore;

        private LineRenderer _line;

        [Inject]
        public void Construct(IHighscore highscore)
        {
            _highscore = highscore;
            _line = GetComponent<LineRenderer>();

            _line.positionCount = 2;
            _line.useWorldSpace = true;
        }

        private void LateUpdate()
        {
            if (!_highscore.Available) return;
            Vector2 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector2 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));

            _line.SetPosition(0, new Vector3(leftEdge.x, _highscore.High, 0));
            _line.SetPosition(1, new Vector3(rightEdge.x, _highscore.High, 0));
        }
    }
}