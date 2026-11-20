using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PaintDecal : MonoBehaviour
{
    [SerializeField] private DecalProjector projector;
    [SerializeField] private UI_PaintDraw paint;
    [SerializeField]private Material decal;
    [SerializeField] private Texture2D saved;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
        decal = new Material(projector.material);
        projector.material = decal;
    }


    public void SetPainter(UI_PaintDraw window) { paint = window; }

    public void ApplyPainting()
    {
        Texture2D texture = paint.GetPaintTexture();
        decal.SetTexture("_Overlay_Map", texture);
        Texture2D copy = new(texture.width, texture.height, TextureFormat.RGBA32, false);
        copy.SetPixels(texture.GetPixels());
        copy.Apply();
        string propertyName = "_Overlay_Map";
        decal.SetTexture(propertyName, copy);
        saved = copy;
        paint = null;
    }

    private void OnDestroy()
    {
        if (decal != null) { Destroy(decal); }
        if (saved != null) { Destroy(saved); }
    }
}
