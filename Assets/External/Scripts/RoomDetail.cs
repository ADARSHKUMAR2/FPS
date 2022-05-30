using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomDetail : MonoBehaviour
{
    [SerializeField] private TMP_Text roomName;
    public RoomInfo _roomInfo;
    public void RoomDetails(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        roomName.text = roomInfo.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(_roomInfo);
    }
}
