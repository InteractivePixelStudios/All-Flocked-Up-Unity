using System.Collections.Generic;
using UnityEngine;

public class GraffitiSpray : MonoBehaviour
{
    [SerializeField] public Camera cam;
    [SerializeField] public int textureSize = 1024;
    [SerializeField] public int brushSize = 8;
    [SerializeField] public Color currentColor = Color.red;
    [SerializeField] public bool isPainting = false;

    private Dictionary<Renderer, Texture2D> paintTextures = new Dictionary<Renderer, Texture2D>();

    void Update()
    {
        if (Input.GetMouseButton(0) && isPainting == true)
        {
            Debug.Log("Painting at mouse position: " + Input.mousePosition);
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Renderer rend = hit.collider.GetComponent<Renderer>();
                if (rend == null) return;

                Texture2D tex = GetOrCreateTexture(rend);

                Vector2 uv = hit.textureCoord;

                int x = (int)(uv.x * textureSize);
                int y = (int)(uv.y * textureSize);

                Paint(tex, x, y);

                tex.Apply();
            }
        }
    }

    Texture2D GetOrCreateTexture(Renderer rend)
    {
        if (paintTextures.ContainsKey(rend))
            return paintTextures[rend];

        Texture2D tex = new Texture2D(textureSize, textureSize);

        Color[] pixels = new Color[textureSize * textureSize];
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.white;

        tex.SetPixels(pixels);
        tex.Apply();

        rend.material.mainTexture = tex;

        paintTextures.Add(rend, tex);

        return tex;
    }

    void Paint(Texture2D tex, int x, int y)
    {
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                float dist = Mathf.Sqrt(i * i + j * j);

                if (dist > brushSize) continue;

                int px = x + i;
                int py = y + j;

                if (px >= 0 && px < tex.width && py >= 0 && py < tex.height)
                    tex.SetPixel(px, py, currentColor);
            }
        }
    }
}
