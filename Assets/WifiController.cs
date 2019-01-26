using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiController : MonoBehaviour
{
    public bool ActiveOnStartup = true;
    public GameObject WifiRenderer;

    public bool IsActiveRouter
    {
        get { return WifiRenderer?.activeSelf ?? false; }
    }

    public void SetRouterActive(bool active)
    {
        WifiRenderer?.SetActive(active);
    }

    public void ToggleRouterActive()
    {
        SetRouterActive(!IsActiveRouter);
    }
    
    void Start()
    {
        SetRouterActive(ActiveOnStartup);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
