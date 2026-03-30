using FMODUnity;
using UnityEngine;

public class Enemy_AlertIcon : MonoBehaviour
{
    [SerializeField] GameObject icon;
    [SerializeField] GameObject outline;
    [SerializeField] private GameObject fillImage;
    private bool playerSeen;
    bool soundPlayed;
    [SerializeField] EventReference alertEvent;

    public void SetPlayerSeen(bool value)
    {
        playerSeen = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSeen)
        {
            ShowIcon();
            MoveImage();

        }
        else HideIcon();
    }

    void MoveImage()
    {
        var pos = fillImage.transform.localPosition.y;
           pos = Mathf.Lerp(-12, -3, 2f);
    }

    void ShowIcon()
    {
        if (!soundPlayed)
        {
            AudioWizard.Instance.PlayOneshotSound(alertEvent, transform.position);
            soundPlayed = true;
        }
        icon.SetActive(true);
    }

    void HideIcon()
    {
        icon.SetActive(false);
        soundPlayed = false;
    }
}
