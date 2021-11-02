using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_Slider : MonoBehaviour
{
    public Slider sliderUI;
    public Text text;

    public Image backdrop;

    // Make UI show current slider value
    void Start()
    {
        ShowSliderValue();
    }

    // On slider update update the UI to reflect this change
    public void ShowSliderValue()
    {
        //text.text = "[" + ((int)(sliderUI.value * 100)) + "]";
        text.text = "[" + Mathf.RoundToInt(sliderUI.value * 100) + "]";
    }

    public void ActivateBackdrop()
    {
        backdrop.enabled = true;
    }

    public void DisableBackdrop()
    {
        backdrop.enabled = false;
    }
}