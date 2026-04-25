using UnityEngine;

public class IconRemover : MonoBehaviour
{

    private CompassController compass;
    private MapIcon worldIcon;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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

    private void OnDestroy()
    {
        compass.RemoveIcon(worldIcon);
    }
}
