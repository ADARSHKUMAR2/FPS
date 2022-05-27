using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private string menuName;
    public bool isOpen;
    public string MenuName
    {
        get { return menuName; }
    }
    public void OpenMenu()
    {
        isOpen = true;
        gameObject.SetActive(true);
    }

    public void CloseMenu()
    {
        isOpen = false;
        gameObject.SetActive(false);
    }
}
