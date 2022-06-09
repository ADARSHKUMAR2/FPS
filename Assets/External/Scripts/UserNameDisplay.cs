using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class UserNameDisplay : MonoBehaviour
{
    [SerializeField] private PhotonView PV;
    [SerializeField] private TMP_Text playerNameText;

    private void Start()
    {
        if(PV.IsMine)
            gameObject.SetActive(false);
        playerNameText.text = PV.Owner.NickName;
    }
}
