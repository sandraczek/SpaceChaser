using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class Build : MonoBehaviour
    {
        public BuildData Data;
        private Action _onDestroy;
        private readonly Dictionary<Strut, FixedJoint2D> _joints = new();

        public void Initialize(Action onDestroy)
        {
            _onDestroy = onDestroy;
            _joints.Clear();
        }

        public void Remove()
        {
            foreach (var (strut, joint) in _joints)
            {
                strut.UnregisterAttachment(this);
                Destroy(joint);
            }
            _joints.Clear();

            _onDestroy.Invoke();
        }

        public void ConnectTo(Strut strut)
        {
            var joint = gameObject.AddComponent<FixedJoint2D>();
            _joints.Add(strut, joint);

            joint.enableCollision = false;
            joint.connectedBody = strut.Rb;

            strut.RegisterAttachment(this);
        }

        public void DisconnectFrom(Strut strut)
        {
            if (_joints.ContainsKey(strut))
            {
                strut.UnregisterAttachment(this);
                Destroy(_joints[strut]);
                _joints.Remove(strut);
            }
            else
            {
                Debug.LogWarning("Tried disconnecting a strut but it's not connected");
            }
        }

        public IReadOnlyList<Strut> GetAllStruts()
        {
            return (IReadOnlyList<Strut>)_joints.Keys.AsReadOnlyList();
        }
    }
}