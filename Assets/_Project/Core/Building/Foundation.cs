using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceChaser.Core.Building
{
    public class Foundation : MonoBehaviour
    {
        public FoundationData Data;
        private Action _onDestroy;
        public bool Salvagable = true;
        public bool Static = false;

        private readonly List<Foundation> _contacts = new();

        public void Initialize(Action onDestroy)
        {
            _onDestroy = onDestroy;
            _contacts.Clear();
        }

        public void Remove()
        {
            foreach (var foundation in _contacts)
            {
                foundation.UnregisterContact(this);
            }
            _contacts.Clear();
            _onDestroy.Invoke();
        }

        public void RegisterContact(Foundation foundation)
        {
            _contacts.Add(foundation);
        }
        public void UnregisterContact(Foundation foundation)
        {
            if (_contacts.Contains(foundation))
                _contacts.Remove(foundation);
            else
            {
                Debug.LogWarning("Tried to unregister a foundation but it was not found");
            }
        }
        public IReadOnlyList<Foundation> GetAllContacts() => _contacts;

        public bool HasConnectionToFloor(HashSet<Foundation> visited)
        {
            if (Static) return true;
            visited.Add(this);
            foreach (Foundation contact in _contacts)
            {
                if (visited.Contains(contact)) continue;

                if (contact.HasConnectionToFloor(visited)) return true;
            }

            return false;
        }
        public bool CheckRemove(HashSet<Foundation> dfsBuffer)
        {
            foreach (var neighbor in _contacts)
            {
                dfsBuffer.Clear();
                dfsBuffer.Add(this);

                if (!neighbor.HasConnectionToFloor(dfsBuffer))
                {
                    return false;
                }
            }

            return true;
        }

    }
}