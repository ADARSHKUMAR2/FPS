using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PhotonView PV;

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
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), Vector3.zero,
            Quaternion.identity);
    }
}
