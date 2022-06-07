using System;
using UnityEngine;

namespace External.Scripts
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private GameObject graphics;

        private void Awake()
        {
            graphics.SetActive(false);
        }
    }
}