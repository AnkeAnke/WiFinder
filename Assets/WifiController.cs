using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WifiController : MonoBehaviour
{
    public enum WifiType
    {
        Static,
        OnOff,
        OnOffA,
        OnOffB,
    }

    public WifiType Type = WifiType.OnOff;

    public bool ActiveOnStartup = true;
    public GameObject WifiRenderer;
    public SpriteRenderer WifiIcon;

    public bool IsActiveRouter
    {
        get { return WifiRenderer?.activeSelf ?? false; }
    }

    private void SetRouterActiveInternal(bool active)
    {
        WifiRenderer?.SetActive(active);
    }

    public void SetRouterActive(bool active)
    {
        if (Type == WifiType.Static)
            return;
        SetRouterActiveInternal(active);

        if (Type == WifiType.OnOffA || Type == WifiType.OnOffB)
        {
            foreach (var wifi in FindObjectsOfType<WifiController>().Where(w => w.Type == WifiType.OnOffA || w.Type == WifiType.OnOffB))
            {
                if (wifi.IsActiveRouter && wifi.Type != Type)
                    wifi.SetRouterActiveInternal(!active);
                else if (!wifi.IsActiveRouter && wifi.Type == Type)
                    wifi.SetRouterActiveInternal(active);
            }
        }
    }

    public void ToggleRouterActive()
    {
        SetRouterActive(!IsActiveRouter);
    }
    
    void Start()
    {
        SetRouterActive(ActiveOnStartup);

        if (Type == WifiType.OnOffA)
            WifiIcon.color = Color.yellow;
        else if (Type == WifiType.OnOffB)
            WifiIcon.color = Color.blue;
        else if (Type == WifiType.Static)
        {
            WifiIcon.color = new Color(0.3f, 0.3f, 0.3f);
            SetRouterActiveInternal(true);
        }
    }
}
