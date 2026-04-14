using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GraffitiSpray : MonoBehaviour
{
    [SerializeField] public int textureSize = 512;

    [SerializeField] private Texture2D paintTexture;
    [SerializeField] private DecalProjector projector;

    void Start()
    {
        projector = GetComponent<DecalProjector>();
        // Grab the original texture from the material
        Texture2D original = projector.material.GetTexture("Base_Map") as Texture2D;
        
        Debug.Log($"Original texture: {original}, isReadable: {original?.isReadable}");
        //the texture2D used by the projector material needs to be read/write enabled in advanced settings, or else it won't work!!!

        // Duplicate material so it's unique
        projector.material = new Material(projector.material);
        if (original != null && original.isReadable)
        {
            // Copy the existing texture so we paint on top of it
            paintTexture = new Texture2D(original.width, original.height, TextureFormat.RGBA32, false);
            paintTexture.SetPixels(original.GetPixels());
            paintTexture.Apply();
        }
        else
        {
            // Fallback, start with a transparent canvas
            paintTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            Color[] pixels = new Color[textureSize * textureSize];
            for (int i = 0; i < pixels.Length; i++)
                pixels[i] = Color.clear;
            paintTexture.SetPixels(pixels);
            paintTexture.Apply();
        }

        paintTexture.name = "painter";
        projector.material.SetTexture("Base_Map", paintTexture);
    }

    public void Paint(Vector2 uv, Color color, int brushSize)
    {
        int x = (int)(uv.x * textureSize);
        int y = (int)(uv.y * textureSize);

        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                //Debug.Log($"Checking pixel offset ({i}, {j})");
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
