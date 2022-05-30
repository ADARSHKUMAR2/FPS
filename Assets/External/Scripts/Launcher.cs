using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField roomNameInputField;
    [SerializeField] private TMP_Text errorMsg;
    [SerializeField] private TMP_Text roomNameText;
    [SerializeField] private Transform roomListParent;
    [SerializeField] private GameObject roomItemPrefab;
    [SerializeField] private Transform playeristParent;
    [SerializeField] private GameObject playerItemPrefab;
    [SerializeField] private GameObject startGameButton;
    
    public static Launcher Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Debug.Log($"Connecting");
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        Debug.Log($"OnConnectedToServer");
        PhotonNetwork.JoinLobby();

        PhotonNetwork.AutomaticallySyncScene = true; 
    }

    public override void OnJoinedLobby()
    {
        MenuHandler.Instance.OpenMenu("titleMenu");
        Debug.Log($"OnJoinedLobby");
        PhotonNetwork.NickName = $"Player {Random.Range(0, 1000):0000}";
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
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;
        
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
        
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform player in playeristParent)
            Destroy(player.gameObject);
        
        for (int i = 0; i < players.Length; i++) 
            Instantiate(playerItemPrefab,playeristParent).GetComponent<PlayerDetail>().SetPlayerDetails(players[i]);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode,string msg)
    {
        errorMsg.text = $"Room creation failed {msg}";
        MenuHandler.Instance.OpenMenu("errorMenu");
    }

    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuHandler.Instance.OpenMenu("loadingMenu");
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuHandler.Instance.OpenMenu("loadingMenu");
    }

    public override void OnLeftRoom()
    {
        MenuHandler.Instance.OpenMenu("titleMenu");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform room in roomListParent) 
            Destroy(room.gameObject);

        for (int i = 0; i < roomList.Count; i++)
        {
            if(roomList[i].RemovedFromList)
                continue;
            Instantiate(roomItemPrefab,roomListParent).GetComponent<RoomDetail>().RoomDetails(roomList[i]);
        } 
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerItemPrefab,playeristParent).GetComponent<PlayerDetail>().SetPlayerDetails(newPlayer);
    }

    #region GameStart

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);
    }

    #endregion
}
