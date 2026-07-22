using System;
using System.Collections.Generic;
using SpaceChaser.Core.Building;
using UnityEditor;
using UnityEngine;

namespace SpaceChaser.Core.Islands
{
    public class Island : MonoBehaviour
    {
        [SerializeField] private List<Resource> resources = new();

        private Action _onDestroy;
        public void Initialize(Action onDestroy)
        {
            _onDestroy = onDestroy;

            foreach (var resource in resources)
            {
                resource.Initialize();
            }
        }

        public void Remove()
        {
            _onDestroy.Invoke();
        }

        [ContextMenu("find resources")]
        public void FindResources()
        {
            GetComponentsInChildren(resources);
            EditorUtility.SetDirty(this);
        }
    }
}