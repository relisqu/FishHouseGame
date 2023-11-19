using UnityEngine;

namespace Drops
{
    public class EntityDrop : MonoBehaviour
    {
        public Transform upperPoint;
        public IngredientType IngredientType;
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

        private void Update()
        {
            if (!HasParent()) Rotate();
        }

        private void Rotate()
        {
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed, Space.World);
        }
    }
}