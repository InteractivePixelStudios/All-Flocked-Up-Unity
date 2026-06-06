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
    bool isShowing;

    public void SetPlayerSeen(bool value)
    {
        playerSeen = value;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerSeen && !isShowing)
        {
            ShowIcon();
            MoveImage();
            isShowing = true;
        }
        else if (!playerSeen && isShowing)
        {
            HideIcon();
            isShowing = false;
        }
    }

    void MoveImage()
    {
        Vector3 pos = fillImage.transform.localPosition;
        pos.y = Mathf.Lerp(pos.y, -3f, Time.deltaTime * 5f);
        fillImage.transform.localPosition = pos;
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
