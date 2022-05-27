using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private List<Menu> menus;

    public static MenuHandler Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menuName == menus[i].MenuName)
                OpenMenu(menus[i]);
            else if(menus[i].isOpen)
                CloseMenu(menus[i]);
        }
    }

    public void OpenMenu(Menu menu)
    {
        for (int i = 0; i < menus.Count; i++)
        {
            if (menus[i].isOpen)
                CloseMenu(menus[i]);
        }
        menu.OpenMenu();
    }

    public void CloseMenu(Menu menu)
    {
        menu.CloseMenu();
    }
}
