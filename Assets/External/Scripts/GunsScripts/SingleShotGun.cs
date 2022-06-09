using System;
using Photon.Pun;
using UnityEngine;

namespace External.Scripts
{
    public class SingleShotGun : Gun
    {
        [SerializeField] private Camera cam;
        private PhotonView PV;

        private void Awake()
        {
            PV = GetComponent<PhotonView>();
        }

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
                PV.RPC("RPC_Shoot",RpcTarget.All,hit.point,hit.normal);
            }
        }

        [PunRPC]
        private void RPC_Shoot(Vector3 hitPos,Vector3 hitNormal)
        {
            Collider[] colliders = Physics.OverlapSphere(hitPos, 0.3f);
            if (colliders.Length!=0)
            {
                GameObject bulletImpactObj =   Instantiate(bulletImpactPrefab, hitPos + hitNormal*0.001f, Quaternion.LookRotation(hitNormal,Vector3.up) * bulletImpactPrefab.transform.rotation);
                bulletImpactObj.transform.SetParent(colliders[0].transform);
                Destroy(bulletImpactObj,10f);
            }
        }
    }
}