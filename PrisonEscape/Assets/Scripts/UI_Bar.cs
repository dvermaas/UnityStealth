using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Bar : MonoBehaviour
{
    public RectTransform HeatFill;
    // GetComponent<RectTransform>();
    // Update is called once per frame
    //private float highest_val;

    public void UpdateBar(float value)
    {
        HeatFill.localScale = new Vector3(value/100, 1f, 1f);
    }
}
