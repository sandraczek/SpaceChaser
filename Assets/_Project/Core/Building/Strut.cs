using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Strut : MonoBehaviour
    {
        public StrutData Data;
        private Action _onDestroy;

        private readonly List<Build> _buildsAttached = new();



        [HideInInspector] public Rigidbody2D Rb;

        public void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();

            //Rb.inertia = 1f;
        }
        public void Initialize(Action onDestroy)
        {
            _onDestroy = onDestroy;

            _buildsAttached.Clear();
        }

        public void Remove()
        {
            _onDestroy.Invoke();
            _buildsAttached.Clear();
        }

        public void RegisterAttachment(Build build)
        {
            _buildsAttached.Add(build);
        }
        public void UnregisterAttachment(Build build)
        {
            if (_buildsAttached.Contains(build))
                _buildsAttached.Remove(build);
            else
            {
                Debug.LogWarning("Tried to unregister attachment, but it was not found");
            }
        }

        public IReadOnlyList<Build> GetAttached()
        {
            return _buildsAttached;
        }
    }
}