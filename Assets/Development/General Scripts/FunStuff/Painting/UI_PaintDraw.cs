using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UI_PaintDraw : MonoBehaviour, IPointerDownHandler,IDragHandler,IPointerUpHandler
{
    [SerializeField] private RawImage drawingCanvas;
    [SerializeField] private int textureSize = 512;
    [SerializeField]private Texture2D paintTexture;
    private Color brushColor = Color.red;
    private int brushSize = 8;
    private Vector2Int? previousPixel;

    private void Start()
    {
        paintTexture = new Texture2D( textureSize,textureSize, TextureFormat.RGBA32, false );
        paintTexture.filterMode = FilterMode.Bilinear;
        ClearCanvas();
        drawingCanvas.texture = paintTexture;
    }

    public void SetBrushColor(Color color)
    {
        brushColor = color;
    }

    public void SetBrushSize(float size)
    {
        brushSize = Mathf.Max(1, Mathf.RoundToInt(size));
    }

    public Texture2D GetPaintTexture()
    {
        return paintTexture;
    }

    public void ClearCanvas()
    {
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.clear;
        }
        paintTexture.SetPixels(pixels);
        paintTexture.Apply();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        previousPixel = null;
        Paint(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Paint(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        previousPixel = null;
    }

    private void Paint(PointerEventData eventData)
    {
        RectTransform rect = drawingCanvas.rectTransform;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle( rect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint)) return;
        Rect bounds = rect.rect;
        float u = (localPoint.x - bounds.xMin) / bounds.width;
        float v = (localPoint.y - bounds.yMin) / bounds.height;
        if (u < 0 || u > 1 || v < 0 || v > 1)
        {
            previousPixel = null;
            return;
        }
        Vector2Int pixel = new Vector2Int( Mathf.Clamp(Mathf.FloorToInt(u * textureSize),0,textureSize - 1),Mathf.Clamp(Mathf.FloorToInt(v * textureSize),0,textureSize - 1));
        if (previousPixel.HasValue)
        {
            Vector2Int start = previousPixel.Value;
            int steps = Mathf.Max(Mathf.Abs(pixel.x - start.x),Mathf.Abs(pixel.y - start.y));
            for (int i = 0; i <= steps; i++)
            {
                float t = steps == 0 ? 0 : (float)i / steps;
                Vector2Int point = Vector2Int.RoundToInt(Vector2.Lerp(start, pixel, t));
               DrawCircle(point);
            }
        }
        else
        {
            DrawCircle(pixel);
        }
        previousPixel = pixel;
        paintTexture.Apply();
    }

    private void DrawCircle(Vector2Int center)
    {
        int radius = Mathf.Max(1, brushSize);
        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y > radius * radius)continue;
                int px = center.x + x;
                int py = center.y + y;
                if (px < 0 || px >= textureSize ||py < 0 || py >= textureSize)continue;
                paintTexture.SetPixel(px, py, brushColor);
            }
        }
    }
}
