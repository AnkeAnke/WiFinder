using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject Bar;
    public float Progress = 1.0f;

    void Start()
    {
    }

    void Update()
    {
        transform.localScale = new Vector2(Progress, 1);
    }
}
