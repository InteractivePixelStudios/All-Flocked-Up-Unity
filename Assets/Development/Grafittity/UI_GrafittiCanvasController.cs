using UnityEngine;
using UnityEngine.UI;

public class UI_GrafittiCanvasController : MonoBehaviour
{
    public Slider slider;
    public Image targetImage;

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateColor);
        UpdateColor(slider.value);
    }
    
    //change color based on slider value
    void UpdateColor(float value)
    {
        float hue = value; // 0 → 1
        Color rainbowColor = Color.HSVToRGB(hue, 1f, 1f);

        targetImage.color = rainbowColor;
    }
}
