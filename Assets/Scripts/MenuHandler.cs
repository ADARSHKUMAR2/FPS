using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private GameObject loadingObj;
    
    public void OpenMenu()
    {
        loadingObj.SetActive(true);
    }

    public void CloseMenu()
    {
        loadingObj.SetActive(false);
    }
}
