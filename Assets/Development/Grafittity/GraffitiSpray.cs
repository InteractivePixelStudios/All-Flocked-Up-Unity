using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GraffitiSpray : MonoBehaviour
{
    [SerializeField] public int textureSize = 512;

    [SerializeField] private Texture2D paintTexture;
    [SerializeField] private DecalProjector projector;

    void Start()
    {
        projector = GetComponent<DecalProjector>();

        paintTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);

        // Start transparent
        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        paintTexture.SetPixels(pixels);
        paintTexture.Apply();

        // Duplicate material so it's unique
        projector.material = new Material(projector.material);
        projector.material.SetTexture("_BaseMap", paintTexture);
    }

    public void Paint(Vector2 uv, Color color, int brushSize)
    {
        int x = (int)(uv.x * textureSize);
        int y = (int)(uv.y * textureSize);

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                Debug.Log($"Checking pixel offset ({i}, {j})");
                float dist = Mathf.Sqrt(i * i + j * j);
                if (dist > brushSize) continue;

                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < textureSize && py >= 0 && py < textureSize)
                {
                    Color existing = paintTexture.GetPixel(px, py);
                    Color blended = Color.Lerp(existing, color, 0.6f);
                    paintTexture.SetPixel(px, py, blended);
                }
            }
        }

        paintTexture.Apply();
    }
}
