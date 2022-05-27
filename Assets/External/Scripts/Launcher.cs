using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text errorMsg;
    
    void Start()
    {
        Debug.Log($"Connecting");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log($"OnConnectedToServer");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        MenuHandler.Instance.OpenMenu("titleMenu");
        Debug.Log($"OnJoinedLobby");
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
            return;

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuHandler.Instance.OpenMenu("loadingMenu");
    }

    public override void OnJoinedRoom()
    {
        MenuHandler.Instance.OpenMenu("roomMenu");
    }

    public override void OnCreateRoomFailed(short returnCode,string msg)
    {
        errorMsg.text = $"Room creation failed {msg}";
        MenuHandler.Instance.OpenMenu("errorMenu");
    }
}
