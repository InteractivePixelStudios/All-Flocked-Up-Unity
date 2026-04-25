using UnityEngine;
using UnityEngine.UI;

public class UI_CBColorFinder : MonoBehaviour
{
    public Color defaultColor;
    private Graphic g;

    void Awake()
    {
        g = GetComponent<Graphic>();
        defaultColor = g.color;
    }

    public void Reset()
    {
        g.color = defaultColor;
    }
}
