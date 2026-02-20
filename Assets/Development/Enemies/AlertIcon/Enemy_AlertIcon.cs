using UnityEngine;

public class Enemy_AlertIcon : MonoBehaviour
{
    [SerializeField] private GameObject fillImage;
    public bool playerSeen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSeen)
        {
            MoveImage();
        }
    }

    void MoveImage()
    {
        var pos = fillImage.transform.localPosition.y;
           pos = Mathf.Lerp(-12, -3, 2f);
    }
}
