using UnityEngine;

namespace Drops
{
    public class GarbageBin : MonoBehaviour
    {
        private void Update()
        {
            _triggerTimer -= Time.deltaTime;
        }

        private float _triggerTimer;

        private void OnTriggerStay(Collider other)
        {
            if (Input.GetKey(KeyCode.E) && _triggerTimer <= 0f)
            {
                if (other.gameObject.TryGetComponent(out PlayerBagPack pack))
                {
                    var item = pack.HasItems("");
                    if (item == null) return;
                    pack.RemoveItem(item);
                    Destroy(item.gameObject);
                    _triggerTimer = 0.4f;
                }
            }
        }
    }
}