using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreboardItem : MonoBehaviour
{
    [SerializeField] private TMP_Text userNameText;
    [SerializeField] private TMP_Text killsText;
    [SerializeField] private TMP_Text deathText;

    public void Initialize(Player player)
    {
        userNameText.text = player.NickName;
    }
}
