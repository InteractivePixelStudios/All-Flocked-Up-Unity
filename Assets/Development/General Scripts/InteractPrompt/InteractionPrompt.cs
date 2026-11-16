using UnityEngine;
using UnityEngine.InputSystem;
public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite controller;
    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        HideIcon();
    }

    public void ShowIcon()
    {
        rend.gameObject.SetActive(true);
        if (Gamepad.current.enabled)
        {
            rend.sprite = controller;
        }
        else
        {
            rend.sprite = keyboard;
        }
    }

    public void HideIcon()
    {
        rend.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ShowIcon();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var cam = Camera.main;
            if (!cam) return;
            rend.gameObject.transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideIcon();
        }
    }
}
