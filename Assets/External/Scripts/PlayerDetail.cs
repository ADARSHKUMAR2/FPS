using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerDetail : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_Text playerNameText;
    private Player playerInfo;

    public void SetPlayerDetails(Player playerInfo)
    {
        this.playerInfo = playerInfo;
        playerNameText.text = playerInfo.NickName;
    }
    
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if(playerInfo == otherPlayer)
            Destroy(gameObject);
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }
}
