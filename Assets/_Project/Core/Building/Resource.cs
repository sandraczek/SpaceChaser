using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class Resource : MonoBehaviour
    {
        public ResourceData Data;
        public void Remove()
        {
            gameObject.SetActive(false);
        }

        public void Initialize()
        {
            gameObject.SetActive(true);
        }
    }
}