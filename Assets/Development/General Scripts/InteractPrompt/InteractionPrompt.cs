using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionPrompt : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private Sprite keyboard;
    [SerializeField] private Sprite controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            var direction = other.transform.position - transform.position;
            Quaternion angle = Quaternion.LookRotation(-direction, Vector3.up);
            rend.gameObject.transform.rotation = angle;
            
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
