using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileWifi : MonoBehaviour
{
    public ProgressBar MobileDataBar;
    public GameObject Wifi;
    public float StartPrepaidBalance;
    public float BalanceLossPerSecond;

    private float _balance;
    

    // Start is called before the first frame update
    void Start()
    {
        Respawn();
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
                Wifi.SetActive(false);
                return;
            }
            Debug.DrawLine(playerPos, wifiPos, Color.white);
        }

        // No unobstructed wifi. Use prepaid.
        Wifi.SetActive(true);
        _balance -= BalanceLossPerSecond * Time.deltaTime;
        if (_balance <= 0)
            Respawn();
        MobileDataBar.Progress = _balance / StartPrepaidBalance;
    }

    void Respawn()
    {
        Wifi.SetActive(false);
        _balance = StartPrepaidBalance;
        GetComponent<Player>().Reset();
    }
}
