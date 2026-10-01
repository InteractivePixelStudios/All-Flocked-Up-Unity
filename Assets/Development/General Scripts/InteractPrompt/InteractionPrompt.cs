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
            //v0, buggy facing player
            // var direction = other.transform.position - transform.position;
            //Quaternion angle = Quaternion.LookRotation(-direction, Vector3.up);
            // rend.gameObject.transform.rotation = angle;

            //v1, icon faces main cam 
            /*
            var cam = Camera.main;
            if (!cam) return;
            var direction = cam.transform.position - transform.position;
            Quaternion angle = Quaternion.LookRotation(-direction, Vector3.up);
            rend.gameObject.transform.rotation = angle; 
            */


            //v2, icon parallel to main cam
            // /* 
            var cam = Camera.main;
            if (!cam) return;
            rend.gameObject.transform.rotation = Quaternion.LookRotation(cam.transform.forward, Vector3.up);
            // */

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
