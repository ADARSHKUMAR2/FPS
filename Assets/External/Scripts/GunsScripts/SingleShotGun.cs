using UnityEngine;

namespace External.Scripts
{
    public class SingleShotGun : Gun
    {
        [SerializeField] private Camera cam;
        
        public override void Use()
        {
            // Debug.Log($"Using Gun - {itemInfo.itemName}");
            Shoot();
        }

        private void Shoot()
        {
            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
            ray.origin = cam.transform.position;
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log($"We hit - {hit.collider.gameObject.name}");
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
            }
        }
    }
}