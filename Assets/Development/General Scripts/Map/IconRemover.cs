using UnityEngine;
public class IconRemover : MonoBehaviour
{
    private CompassController compass;
    private MapIcon worldIcon;
    void Start()
    {
        compass = FindAnyObjectByType<CompassController>();
    }

    public void SetCompassRef(CompassController reference)
    {
        compass = reference;
    }

    public void SetIconRef(MapIcon reference)
    {
        worldIcon = reference;
    }

    public void DestroyIcon()
    {
        compass.RemoveIcon(worldIcon);
    }
}
