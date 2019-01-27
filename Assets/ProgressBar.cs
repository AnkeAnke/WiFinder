using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public GameObject Bar;
    public float Balance;
    public float DecreasePerSecond = 1f;
    // Start is called before the first frame update
    void Start()
    {
        var fullRect = GetComponent<RectTransform>();
        float fullWidth = fullRect.sizeDelta.x;
        //float fullHeight = fullRect.sizeDelta.y;
        float fullLeft = fullRect.offsetMin.x;

        var barRect = Bar.GetComponent<RectTransform>();
        float barWidth = barRect.sizeDelta.x;
        //float fullHeight = fullRect.sizeDelta.y;
        float barLeft = barRect.offsetMin.x;
        Vector3 barPos = barRect.position;

        //float spacing = 0.5f * fullWidth / (fullWidth - fullLeft);
        //int numBars = (int)(fullWidth / spacing);
        //Debug.Log("Num Bars: " + numBars);
        //for (int b = 1; b < numBars; ++b)
        //{
        //    var newBar = Instantiate(Bar, new Vector3(barPos.x + b * spacing, barPos.y, barPos.z), Quaternion.identity);
        //    newBar.SetActive(true);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        Balance -= DecreasePerSecond * Time.deltaTime;
    }
}
