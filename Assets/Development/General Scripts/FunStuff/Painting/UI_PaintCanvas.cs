using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class UI_PaintCanvas : MonoBehaviour
{
    [SerializeField] private UI_CanvasController canvasController;
    [SerializeField] private UI_PaintDraw paintCanvas;
    [SerializeField] private PaintInteraction paintInteraction;
    [SerializeField] private PaintDecal paintDecal;
    [SerializeField] private Slider sizeSlider;
    [SerializeField] private Slider colorSlider;
    [SerializeField] private Button doneButton;
    [SerializeField] private Button cancelButton;
    Color currentColor = Color.white;
    float currentSize = 1f;

    public void SetCanvasController(UI_CanvasController canvas)
    {
        canvasController = canvas;
    }

    public void SetPaintInteraction(PaintInteraction interact)
    {
        paintInteraction = interact;
    }

    public void SetDecal(PaintDecal decal)
    {
        paintDecal = decal;
        SetPaintWindow();
    }

    public void SetPaintWindow()
    {
        paintDecal.SetPainter(paintCanvas);
    }

    public Color GetCurrentColor()
    {
        return currentColor;
    }

    public float GetCurrentSize()
    {
        return currentSize;
    }

    private void Start()
    {
        if (paintCanvas == null) { CallCancelPainting(); return; }
        doneButton.onClick.AddListener(CallDonePainting);
        cancelButton.onClick.AddListener(CallCancelPainting);
        sizeSlider.onValueChanged.AddListener(ChangeSize);
        colorSlider.onValueChanged.AddListener(GetColor);
        ChangeSize(sizeSlider.value);
        GetColor(colorSlider.value);
    }

    private void ChangeSize(float size)
    {
        float clampedSize = Mathf.Clamp(size,sizeSlider.minValue,sizeSlider.maxValue);
        sizeSlider.handleRect.localScale = Vector3.one * clampedSize;
        currentSize = clampedSize;
        paintCanvas.SetBrushSize(size);
    }

    private void GetColor(float value)
    {
        float hue = Mathf.InverseLerp(colorSlider.minValue,colorSlider.maxValue,value);
        ChangeColor(Color.HSVToRGB(hue, 1f, 1f));
    }

    private void ChangeColor(Color color)
    {
        colorSlider.handleRect.GetComponent<Image>().color = color;
        currentColor = color;
        paintCanvas.SetBrushColor(color);
    }

    private void CallDonePainting()
    {
        paintDecal.ApplyPainting();
        paintInteraction.StopPainting();
        canvasController.ClosePaint();
    }

    private void CallCancelPainting()
    {
        canvasController.ClosePaint();
        paintInteraction.StopPainting();
    }

    private void OnDestroy()
    {
        sizeSlider.onValueChanged.RemoveListener(ChangeSize);
        colorSlider.onValueChanged.RemoveListener(GetColor);
        doneButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

}
