using NUnit.Framework;
using UnityEngine;

public class NestBase : MonoBehaviour
{
    [Header("BaseClassRefs")]
    private GameObject playerRef;
    [SerializeField] private float activeRadius;
    [SerializeField] private StickBuilder stickBuilder;
    public bool isActiveNest;
    private UI_CanvasController canvasController;


    protected void ShowNest()
    {
        gameObject.SetActive(true);
    }

    protected void HideNest()
    {
        gameObject.SetActive(false);
    }


    public void InteractWithNest()
    {
        if (!isActiveNest)
        {
            SpawnStickBuilder();
        }
        else OpenNestMenu();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerRef = other.gameObject;
            ShowNest();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HideNest();
        }
    }

    private void SpawnStickBuilder()
    {
        stickBuilder = Instantiate(stickBuilder, transform);
        stickBuilder.nestBaseRef = this;
    }

    private void OpenNestMenu()
    {
        if (isActiveNest)
        {
            canvasController = FindFirstObjectByType<UI_CanvasController>();
            canvasController.OpenNestMenu();
        }
    }

}
