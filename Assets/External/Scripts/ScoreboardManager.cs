using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class ScoreboardManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private Transform container;
    [SerializeField] private GameObject scoreboardItemPrefab;

    private Dictionary<Player, ScoreboardItem> _scoreboardDictionary = new Dictionary<Player, ScoreboardItem>();
    private void Start()
    {
        foreach (var player in PhotonNetwork.PlayerList)
            AddScoreboardItem(player);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player player)
    {
        RemoveScoreboardItem(player);
    }
    private void AddScoreboardItem(Player player)
    {
        ScoreboardItem scoreboardItem = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        scoreboardItem.Initialize(player);
        _scoreboardDictionary[player] = scoreboardItem;
    }
    
    private void RemoveScoreboardItem(Player player)
    {
        Destroy(_scoreboardDictionary[player].gameObject);
        _scoreboardDictionary.Remove(player);
    }

}
