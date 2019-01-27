using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileWifi : MonoBehaviour
{
    public GameObject MobileDataBar;
    public GameObject Wifi;
    public float PrepaidBalance;

    private ProgressBar _mobileData;
    private Vector2 _spawnPosition;
    private float _spawnBalance;

    // Start is called before the first frame update
    void Start()
    {
        _mobileData = MobileDataBar.GetComponent<ProgressBar>();
        SetMobileDataActive(false);
        _spawnBalance = _mobileData.Balance;
        _spawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Wifi.SetActive(true);
        WifiRenderer[] wifis = FindObjectsOfType<WifiRenderer>();
        Vector2 playerPos = transform.position;

        foreach (var wifi in wifis)
        {
            if (wifi.gameObject == Wifi)
                continue;
            // Check whether within circle.
            Vector2 wifiPos = wifi.transform.position;
            float wifiRadius = wifi.transform.lossyScale.x * 0.5f * 0.9f;
            if ((wifiPos - playerPos).magnitude >= wifiRadius)
                continue;

            // Raycast against ground.
            int groundMask = 1 << LayerMask.NameToLayer("Ground");
            RaycastHit2D hit = Physics2D.Linecast(playerPos, wifiPos, groundMask);
            if (hit.collider == null)
            {
                Debug.DrawLine(playerPos, wifiPos, Color.red);
                SetMobileDataActive(false);
                return;
            }
            Debug.DrawLine(playerPos, wifiPos, Color.white);
        }

        // No unobstructed wifi. Use prepaid.
        SetMobileDataActive(true);
        if (_mobileData.Balance <= 0)
            Respawn();
    }

    void SetMobileDataActive(bool active)
    {
        MobileDataBar.SetActive(active);
        Wifi.SetActive(active);
    }

    void Respawn()
    {
        //_mobileData.Balance = _spawnBalance;
        //MobileDataBar.SetActive(false);
        //transform.position = _spawnPosition;
    }
}
