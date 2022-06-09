using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace External.Scripts
{
    public class PlayerNameManager : MonoBehaviour
    {
        [SerializeField] private TMP_InputField userNameInput;

        private void Start()
        {
            if (PlayerPrefs.HasKey("userName"))
            {
                userNameInput.text = PlayerPrefs.GetString("userName");
                SetName();
            }
            else
            {
                userNameInput.text = $"Player {Random.Range(0, 1999):0000}";
                OnUserNameInputChanged();
            }
        }

        public void OnUserNameInputChanged()
        {
            SetName();
            PlayerPrefs.SetString("userName", userNameInput.text);
        }

        private void SetName()
        {
            PhotonNetwork.NickName = userNameInput.text;
        }
    }
}