using UnityEngine;

namespace External.Scripts
{
    public class SingleShotGun : Gun
    {
        public override void Use()
        {
            Debug.Log($"Using Gun - {itemInfo.itemName}");
        }
    }
}