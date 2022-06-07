using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using External.Scripts;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;
    private GameObject playerController;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    private void Start()
    {
        if(PV.IsMine)
            CreateController();
    }

    private void CreateController()
    {
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint();
        playerController = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position,
            Quaternion.identity ,0 , new object[] {PV.ViewID});
    }

    public void Die()
    {
        PhotonNetwork.Destroy(playerController);
        CreateController();
    }
}
