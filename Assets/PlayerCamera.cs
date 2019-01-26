using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform Player;
    public float AdjustmentSpeed = 15.0f;
    
    void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().AddForce((Player.transform.position - transform.position) * AdjustmentSpeed);
    }
}
