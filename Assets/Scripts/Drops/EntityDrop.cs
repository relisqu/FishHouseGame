using UnityEngine;

namespace Drops
{
    public class EntityDrop : MonoBehaviour
    {
        public Transform upperPoint;
        private Transform _parent;
        [SerializeField] private float rotationSpeed;

        public void SetParent(Transform parentTransform)
        {
            _parent = parentTransform;
            transform.parent = parentTransform;
        }

        public void MoveToParent()
        {
            transform.position = _parent.position;
        }

        public void FollowParent(float distance)
        {

        }

        public bool HasParent()
        {
            return _parent != null;
        }

    }
}