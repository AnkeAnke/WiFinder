using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform Player;
    public float OffsetUp = 10;
    public float AdjustmentSpeed = 15.0f;
    
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce((Player.transform.position + Vector3.up * OffsetUp - transform.position) * AdjustmentSpeed);
    }
}
